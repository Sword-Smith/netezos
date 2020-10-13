using Netezos.Rpc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

namespace ConsoleClient
{
    internal sealed class Program
    {
        // For e.g. the public Carthage test net, the test net parameter is false. It is only
        // true for local sandboxed instances of Tezos blockchains
        private static bool useTestNet = false;
        private static readonly TezosRpc Rpc = new TezosRpc(
            ConfigurationManager.AppSettings["Tezos_Node_DaemonUrl_Testnet"],
            useTestNet ? Chain.Test : Chain.Main);

        private static void Main()
        {
            try
            {
                string headBlockHash = Rpc.Blocks.Head.Hash.GetAsync().Result.Value<string>();
                //var counter = Rpc.Blocks.Head.Context.Contracts["tz1VxS7ff4YnZRs8b4mMP4WaMVpoQjuo1rjf"].Counter.GetAsync<int>().Result;
                //Console.WriteLine($"Head hash is: {headBlockHash}");

                //var tx = new
                //{
                //    kind = "transaction",
                //    source = "tz1VxS7ff4YnZRs8b4mMP4WaMVpoQjuo1rjf",
                //    fee = "1288",
                //    counter = (++counter).ToString(CultureInfo.InvariantCulture),
                //    gas_limit = (11000 + 100).ToString(CultureInfo.InvariantCulture),
                //    storage_limit = "0",
                //    amount = (1).ToString(CultureInfo.InvariantCulture),
                //    destination = "tz1VxS7ff4YnZRs8b4mMP4WaMVpoQjuo1rjf"
                //};

                //List<object> txList = new List<object>();
                //txList.Add(tx);
                //string rawHex = Rpc.Blocks.Head.Helpers.Forge.Operations.PostAsync<string>(headBlockHash, txList).Result;
                //Console.WriteLine($"Transaction payload was {rawHex}");

                //          [ { "protocol": "PsCARTHAGazKbHtnKfLzQg3kms52kSRpgnDY982a9oYsSXRLQEb",
                //"chain_id": "NetXjD3HPJJjmcd",
                //"hash": "ooyLz2EhcvaUXU34kRjN6UmZDTjby1PHA6HePCTNqL7FRcWiMzU",
                //"branch": "BMe7cKu7ibsuRBekqfa71u4A3bvPhjcsYSbAxpDhwXCMTq28VVk",
                //"contents":
                //  [ { "kind": "transaction",
                //      "source": "tz1NwWopSpZ192N9h7yAtERiDWrEKKTVbHKY", "fee": "1420",
                //      "counter": "596656", "gas_limit": "10600",
                //      "storage_limit": "300", "amount": "2760359",
                //      "destination": "tz1hZqcve3zPNokxgpQYcm6yPcaTJhznxMBB",
                //      "metadata":
                //        { "balance_updates":
                //            [ { "kind": "contract",
                //                "contract": "tz1NwWopSpZ192N9h7yAtERiDWrEKKTVbHKY",
                //                "change": "-1420" },
                //              { "kind": "freezer", "category": "fees",
                //                "delegate": "tz1VxS7ff4YnZRs8b4mMP4WaMVpoQjuo1rjf",
                //                "cycle": 360, "change": "1420" } ],
                //          "operation_result":
                //            { "status": "applied",
                //              "balance_updates":
                //                [ { "kind": "contract",
                //                    "contract": "tz1NwWopSpZ192N9h7yAtERiDWrEKKTVbHKY",
                //                    "change": "-2760359" },
                //                  { "kind": "contract",
                //                    "contract": "tz1hZqcve3zPNokxgpQYcm6yPcaTJhznxMBB",
                //                    "change": "2760359" } ], "consumed_gas": "10207" } } } ],
                //"signature":
                //  "sigoLQNqAUjRvFBwLQH6fjofvLYdk5LMLQtDRM1Hy6YkcnZSckBjthohTVEaGT2pJAZ2Xfvy3TqZu4k16ZEzTtZs2518uZGQ" },

                // Tx object from Taquito unit tests
                var newTx = new
                {
                    kind = "transaction",
                    counter = "596656",
                    source = "tz1NwWopSpZ192N9h7yAtERiDWrEKKTVbHKY",
                    fee = "1420",
                    gas_limit = "10600",
                    storage_limit = "300",
                    amount = "2760359",
                    destination = "tz1hZqcve3zPNokxgpQYcm6yPcaTJhznxMBB"
                };
                var newTxList = new List<object>();
                newTxList.Add(newTx);
                string newRawHex = Rpc.Blocks.Head.Helpers.Forge.Operations.PostAsync<string>("BMe7cKu7ibsuRBekqfa71u4A3bvPhjcsYSbAxpDhwXCMTq28VVk", newTxList).Result;
                Console.WriteLine($"Transaction payload was {newRawHex}");

                string fakeSignature = new String('0', 128);
                string data = newRawHex.Substring(64) + fakeSignature;
                //data = "0800002122d44d997e158c36c60649d198c4175dad425efa09d2a405f44e00f6f0b40201420eaa410ac21addf427211cddd6115cba385a94" + fakeSignature;
                var operation = new
                {
                    data = data,
                    branch = "BMe7cKu7ibsuRBekqfa71u4A3bvPhjcsYSbAxpDhwXCMTq28VVk"
                };
                var operations = new
                {
                    operations = new List<object> { operation },
                };
                try
                {
                    Console.WriteLine($"Data: \"{data}\"");
                    var resp = Rpc.Blocks.Head.Helpers.Parse.Operations.PostAsync<object>(operations).Result;
                    Console.WriteLine($"Data: \"{data}\"");
                    Console.WriteLine($"Data: \"{data}\"");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }




                //{
                //    name: 'Transaction',
                //    operation: {
                //      branch: 'BLzyjjHKEKMULtvkpSHxuZxx6ei6fpntH2BTkYZiLgs8zLVstvX',
                //      contents: [
                //        {
                //          kind: 'transaction',
                //          counter: '1',
                //          source: 'tz1QZ6KY7d3BuZDT1d19dUxoQrtFPN2QJ3hn',
                //          fee: '10000',
                //          gas_limit: '10',
                //          storage_limit: '10',
                //          destination: 'tz1QZ6KY7d3BuZDT1d19dUxoQrtFPN2QJ3hn',
                //          amount: '1000',
                //        },
                //      ],
                //    },
                //  },

            }
            catch (Exception exception)
            {
                Console.WriteLine("[Failed]\n\nPlease check your configuration and make sure that the daemon is up and running and that it is synchronized. \n\nException: " + exception);
            }
        }
    }
}