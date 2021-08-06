using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData.Binding;
using ReactiveUI;
using WalletWasabi.Blockchain.Analysis.Clustering;
using WalletWasabi.Blockchain.Keys;
using WalletWasabi.Fluent.ViewModels.Navigation;
using WalletWasabi.Fluent.ViewModels.Wallets.Labels;
using WalletWasabi.Wallets;

namespace WalletWasabi.Fluent.ViewModels.Wallets.Receive
{
	[NavigationMetaData(
		Title = "Receive",
		Caption = "",
		IconName = "wallet_action_receive",
		NavBarPosition = NavBarPosition.None,
		Searchable = false,
		NavigationTarget = NavigationTarget.DialogScreen)]
	public partial class ReceiveViewModel : RoutableViewModel
	{
		private readonly Wallet _wallet;
		[AutoNotify] private bool _isExistingAddressesButtonVisible;
		[AutoNotify(SetterModifier = AccessModifier.Private)] private SuggestionLabelsViewModel _suggestionLabels;

		public ReceiveViewModel(Wallet wallet)
		{
			_wallet = wallet;
			SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: true);

			EnableBack = false;

			_suggestionLabels = new SuggestionLabelsViewModel(3);

			NextCommand = ReactiveCommand.Create(OnNext, _suggestionLabels.Labels.ToObservableChangeSet().Select(_ => _suggestionLabels.Labels.Count > 0));

			ShowExistingAddressesCommand = ReactiveCommand.Create(OnShowExistingAddresses);
		}

		public ICommand ShowExistingAddressesCommand { get; }

		private void OnNext()
		{
			var newKey = _wallet.KeyManager.GetNextReceiveKey(new SmartLabel(SuggestionLabels.Labels), out bool minGapLimitIncreased);

			if (minGapLimitIncreased)
			{
				int minGapLimit = _wallet.KeyManager.MinGapLimit.Value;
				int prevMinGapLimit = minGapLimit - 1;
				var minGapLimitMessage = $"Minimum gap limit increased from {prevMinGapLimit} to {minGapLimit}.";

				// TODO: notification
			}

			_suggestionLabels.Labels.Clear();

			Navigate().To(new ReceiveAddressViewModel(_wallet, newKey));
		}

		private void OnShowExistingAddresses()
		{
			Navigate().To(new ReceiveAddressesViewModel(_wallet, _suggestionLabels.Suggestions.ToHashSet()));
		}

		protected override void OnNavigatedTo(bool isInHistory, CompositeDisposable disposable)
		{
			base.OnNavigatedTo(isInHistory, disposable);

			IsExistingAddressesButtonVisible = _wallet.KeyManager.GetKeys(x => !x.Label.IsEmpty && !x.IsInternal && x.KeyState == KeyState.Clean).Any();
		}
	}
}
