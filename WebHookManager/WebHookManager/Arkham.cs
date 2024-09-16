namespace WebHookManager;

using System;
using System.Collections.Generic;

public record Arkham(
    Transfer Transfer,
    string AlertName,
    int Id
);

public record Transfer(
    string Id,
    string TransactionHash,
    Address FromAddress,
    bool FromIsContract,
    Address ToAddress,
    bool ToIsContract,
    string TokenAddress,
    string Type,
    DateTime BlockTimestamp,
    int BlockNumber,
    string BlockHash,
    string TokenName,
    string TokenSymbol,
    int TokenDecimals,
    double UnitValue,
    string TokenId,
    double HistoricalUSD,
    string Chain
);

public record Address(
    string AddressValue,  // Renamed from "Address" to "AddressValue"
    string Chain,
    ArkhamEntity ArkhamEntity,
    ArkhamLabel ArkhamLabel,
    bool IsUserAddress,
    bool Contract
);

public record ArkhamEntity(
    string Name,
    string Note,
    string Id,
    string Type,
    string? Service,              // Nullable
    List<string>? Addresses,      // Nullable
    string? Website,              // Nullable
    string? Twitter,              // Nullable
    string? Crunchbase,           // Nullable
    string? Linkedin              // Nullable
);

public record ArkhamLabel(
    string Name,
    string Address,
    string ChainType
);
