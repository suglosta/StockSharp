#region S# License
/******************************************************************************************
NOTICE!!!  This program and source code is owned and licensed by
StockSharp, LLC, www.stocksharp.com
Viewing or use of this code requires your acceptance of the license
agreement found at https://github.com/StockSharp/StockSharp/blob/master/LICENSE
Removal of this comment is a violation of the license agreement.

Project: StockSharp.Algo.Storages.Csv.Algo
File: OrderLogCsvSerializer.cs
Created: 2015, 12, 14, 1:43 PM

Copyright 2010 by StockSharp, LLC
*******************************************************************************************/
#endregion S# License
namespace StockSharp.Algo.Storages.Csv
{
	using System;
	using System.Text;

	using Ecng.Common;

	using StockSharp.Messages;

	/// <summary>
	/// The order log serializer in the CSV format.
	/// </summary>
	public class OrderLogCsvSerializer : CsvMarketDataSerializer<ExecutionMessage>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OrderLogCsvSerializer"/>.
		/// </summary>
		/// <param name="securityId">Security ID.</param>
		/// <param name="encoding">Encoding.</param>
		public OrderLogCsvSerializer(SecurityId securityId, Encoding encoding = null)
			: base(securityId, encoding)
		{
		}

		/// <summary>
		/// To create empty meta-information.
		/// </summary>
		/// <param name="date">Date.</param>
		/// <returns>Meta-information on data for one day.</returns>
		public override IMarketDataMetaInfo CreateMetaInfo(DateTime date)
		{
			return new CsvMetaInfo(date, Encoding, r => r.ReadNullableLong());
		}

		/// <summary>
		/// Write data to the specified writer.
		/// </summary>
		/// <param name="writer">CSV writer.</param>
		/// <param name="data">Data.</param>
		protected override void Write(CsvFileWriter writer, ExecutionMessage data)
		{
			writer.WriteRow(new[]
			{
				data.ServerTime.UtcDateTime.ToString(TimeFormat),
				data.ServerTime.ToString("zzz"),
				data.TransactionId.ToString(),
				data.OrderId.ToString(),
				data.OrderPrice.ToString(),
				data.OrderVolume.ToString(),
				data.Side.ToString(),
				data.OrderState.ToString(),
				data.TimeInForce.ToString(),
				data.TradeId.ToString(),
				data.TradePrice.ToString(),
				data.PortfolioName,
				data.IsSystem.ToString()
			});
		}

		/// <summary>
		/// Load data from the specified reader.
		/// </summary>
		/// <param name="reader">CSV reader.</param>
		/// <param name="date">Date.</param>
		/// <returns>Data.</returns>
		protected override ExecutionMessage Read(FastCsvReader reader, DateTime date)
		{
			return new ExecutionMessage
			{
				SecurityId = SecurityId,
				ExecutionType = ExecutionTypes.OrderLog,
				ServerTime = ReadTime(reader, date),
				TransactionId = reader.ReadLong(),
				OrderId = reader.ReadLong(),
				OrderPrice = reader.ReadDecimal(),
				OrderVolume = reader.ReadDecimal(),
				Side = reader.ReadEnum<Sides>(),
				OrderState = reader.ReadEnum<OrderStates>(),
				TimeInForce = reader.ReadNullableEnum<TimeInForce>(),
				TradeId = reader.ReadNullableLong(),
				TradePrice = reader.ReadNullableDecimal(),
				PortfolioName = reader.ReadString(),
				IsSystem = reader.ReadNullableBool(),
			};
		}
	}
}