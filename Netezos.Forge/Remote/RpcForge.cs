﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Netezos.Rpc;
using Netezos.Forge.Operations;
using Netezos.Forge.Utils;

namespace Netezos.Forge
{
    public class RpcForge : IForge, IDisposable
    {
        readonly TezosRpc Rpc;

        public RpcForge(string uri, int timeout = 30_000, Chain chain = Chain.Main)
            => Rpc = new TezosRpc(uri, timeout, chain);

        public void Dispose() => Rpc.Dispose();

        public Task<byte[]> ForgeOperationAsync(OperationContent content)
            => ForgeAsync(new List<object> { content });

        public Task<byte[]> ForgeOperationAsync(string branch, OperationContent content)
            => ForgeAsync(branch, new List<object> { content });

        public Task<byte[]> ForgeOperationGroupAsync(IEnumerable<ManagerOperationContent> content)
            => ForgeAsync(content.Cast<object>().ToList());

        public Task<byte[]> ForgeOperationGroupAsync(string branch, IEnumerable<ManagerOperationContent> content)
            => ForgeAsync(branch, content.Cast<object>().ToList());

        async Task<byte[]> ForgeAsync(List<object> content)
        {
            var branch = await Rpc.Blocks.Head.Hash.GetAsync<string>();
            return await ForgeAsync(branch, content);
        }

        async Task<byte[]> ForgeAsync(string branch, List<object> content)
        {
            var result = await Rpc
                .Blocks
                .Head
                .Helpers
                .Forge
                .Operations
                .PostAsync<string>(branch, content);

            return Hex.Parse(result);
        }
    }
}
