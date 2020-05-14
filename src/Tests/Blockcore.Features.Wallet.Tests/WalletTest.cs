﻿using System.Collections.Generic;
using System.Linq;
using Blockcore.Features.Wallet.Types;
using Blockcore.Tests.Common;
using NBitcoin;
using Xunit;

namespace Blockcore.Features.Wallet.Tests
{
    public class WalletTest : WalletTestBase
    {
        [Fact]
        public void GetAccountsWithoutAccountsReturnsEmptyList()
        {
            var wallet = new Types.Wallet();

            IEnumerable<HdAccount> result = wallet.GetAccounts();

            Assert.Empty(result);
        }

        [Fact]
        public void GetAllTransactionsReturnsTransactionsFromWallet()
        {
            var wallet = new Types.Wallet();
            AccountRoot stratisAccountRoot = CreateAccountRootWithHdAccountHavingAddresses("StratisAccount", KnownCoinTypes.Stratis);

            TransactionData transaction1 = CreateTransaction(new uint256(1), new Money(15000), 1);
            TransactionData transaction2 = CreateTransaction(new uint256(2), new Money(91209), 1);
            
            stratisAccountRoot.Accounts.ElementAt(0).InternalAddresses.ElementAt(0).Transactions.Add(transaction1);
            stratisAccountRoot.Accounts.ElementAt(0).ExternalAddresses.ElementAt(0).Transactions.Add(transaction2);
            
            wallet.AccountsRoot.Add(stratisAccountRoot);
            
            List<TransactionData> result = wallet.GetAllTransactions().ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(transaction1, result[1]);
            Assert.Equal(transaction2, result[0]);
        }

        [Fact]
        public void GetAllTransactionsWithoutAccountRootReturnsEmptyList()
        {
            var wallet = new Types.Wallet();

            List<TransactionData> result = wallet.GetAllTransactions().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void GetAllPubKeysReturnsPubkeysFromWallet()
        {
            var wallet = new Types.Wallet();
            AccountRoot stratisAccountRoot = CreateAccountRootWithHdAccountHavingAddresses("StratisAccount", KnownCoinTypes.Stratis);
            wallet.AccountsRoot.Add(stratisAccountRoot);

            List<Script> result = wallet.GetAllPubKeys().ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(stratisAccountRoot.Accounts.ElementAt(0).ExternalAddresses.ElementAt(0).ScriptPubKey, result[0]);
            Assert.Equal(stratisAccountRoot.Accounts.ElementAt(0).InternalAddresses.ElementAt(0).ScriptPubKey, result[1]);
        }

        [Fact]
        public void GetAllPubKeysWithoutAccountRootsReturnsEmptyList()
        {
            var wallet = new Types.Wallet();

            List<Script> result = wallet.GetAllPubKeys().ToList();

            Assert.Empty(result);
        }
    }
}