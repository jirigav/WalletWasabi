using System.Collections.Generic;
using System.Linq;
using NBitcoin;
using WalletWasabi.Blockchain.Analysis.Clustering;
using WalletWasabi.Blockchain.Transactions.Summary;
using WalletWasabi.Models;

namespace WalletWasabi.Blockchain.Transactions;

public class TransactionSummary
{
	public DateTimeOffset DateTime { get; set; }
	public Height Height { get; set; }
	public Money Amount { get; set; }
	public SmartLabel Label { get; set; }
	public uint256 TransactionId { get; set; }
	public uint256? BlockHash { get; set; }
	public int BlockIndex { get; set; }
	public bool IsOwnCoinjoin { get; set; }
	public IEnumerable<Output> Outputs { get; set; }
	public IEnumerable<Input> Inputs { get; set; }
	public int VirtualSize { get; set; }
	public int Version { get; set; }
	public long BlockTime { get; set; }
	public int Size { get; set; }
	public Money OutputAmount => Outputs.Sum(x => x.Amount);
	public Money? InputAmount => Inputs.Any(x => x.Amount == null) ? null : Inputs.Sum(x => x.Amount);
	public Money? Fee => InputAmount != null ? InputAmount - OutputAmount : null;
	public bool IsSelfSpend => Fee != null && -Amount == Fee;
}
