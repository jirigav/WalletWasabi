using NBitcoin;
using ReactiveUI;
using System.Collections.Generic;
using System.Windows.Input;
using WalletWasabi.Blockchain.Keys;
using WalletWasabi.Fluent.Models;
using WalletWasabi.Gui;
using WalletWasabi.Gui.ViewModels;
using WalletWasabi.Wallets;

namespace WalletWasabi.Fluent.AddWallet.CreateWallet
{
	public class RecoveryWordsViewModel : ViewModelBase, IRoutableViewModel
	{
		public RecoveryWordsViewModel(IScreen screen, KeyManager keyManager, Mnemonic mnemonic, WalletManager walletManager)
		{
			HostScreen = screen;
			MnemonicWords = new List<RecoveryWord>();

			for (int i = 0; i < mnemonic.Words.Length; i++)
			{
				MnemonicWords.Add(new RecoveryWord(i + 1, mnemonic.Words[i]));
			}

			ContinueCommand = ReactiveCommand.Create(() => screen.Router.Navigate.Execute(new ConfirmRecoveryWordsViewModel(HostScreen, MnemonicWords, keyManager, walletManager)));
		}

		public ICommand ContinueCommand { get; }
		public ICommand CancelCommand { get; }

		public List<RecoveryWord> MnemonicWords { get; set; }

		public string UrlPathSegment { get; }
		public IScreen HostScreen { get; }
	}
}