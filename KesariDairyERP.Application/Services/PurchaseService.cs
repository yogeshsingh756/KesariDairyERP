using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.Purchase;
using KesariDairyERP.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KesariDairyERP.Application.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _repo;

        public PurchaseService(IPurchaseRepository repo)
        {
            _repo = repo;
        }


        //public MilkPurchaseCalculateResponse CalculateMilk(
        //    MilkPurchaseCalculateRequest request)
        //{
        //    // -----------------------------
        //    // 1️⃣ Dynamic MILKY Controls
        //    // -----------------------------
        //    decimal snfOffset;
        //    decimal divisor;

        //    if (request.Fat >= 4.0m)
        //    {
        //        snfOffset = 0.33m;
        //        divisor = 10.0m;
        //    }
        //    else if (request.Fat >= 3.5m)
        //    {
        //        snfOffset = 0.25m;
        //        divisor = 11.0m;
        //    }
        //    else if (request.Fat >= 3.0m)
        //    {
        //        snfOffset = 0.20m;
        //        divisor = 11.5m;
        //    }
        //    else if (request.Fat >= 2.5m)
        //    {
        //        snfOffset = 0.18m;
        //        divisor = 12.2m;
        //    }
        //    else
        //    {
        //        snfOffset = 0.14m;
        //        divisor = 12.8m;
        //    }

        //    // -----------------------------
        //    // 2️⃣ SNF Calculation (MILKY)
        //    // -----------------------------
        //    var snfRaw =
        //        (request.CLR / 4m)
        //        + (0.20m * request.Fat)
        //        + snfOffset;

        //    // MILKY truncates SNF (not round)
        //    var snf = Math.Floor(snfRaw * 100m) / 100m;

        //    // -----------------------------
        //    // 3️⃣ Kg Calculations
        //    // -----------------------------
        //    var fatKg = Math.Round(request.Quantity * request.Fat / 100m, 2);
        //    var snfKg = Math.Round(request.Quantity * snf / 100m, 2);

        //    // -----------------------------
        //    // 4️⃣ Base Rate
        //    // -----------------------------
        //    var baseRate = request.Rate / 100m; // e.g. 5100 -> 51.00

        //    // -----------------------------
        //    // 5️⃣ Avg Rate (SNF-Driven)
        //    // -----------------------------
        //    var avgRateRaw = baseRate * (snf / divisor);
        //    var avgRate = Math.Round(avgRateRaw, 2, MidpointRounding.AwayFromZero);

        //    // -----------------------------
        //    // 6️⃣ Total Amount
        //    // -----------------------------
        //    var totalAmount =
        //        Math.Round(avgRate * request.Quantity, 2, MidpointRounding.AwayFromZero);

        //    // -----------------------------
        //    // 7️⃣ Fat Rate (Display Only)
        //    // -----------------------------
        //    const decimal fatRateDisplay = 7.85m; // MILKY fixed UI value

        //    return new MilkPurchaseCalculateResponse
        //    {
        //        SNFPercent = snf,
        //        FatKg = fatKg,
        //        SNFKg = snfKg,
        //        FatRate = fatRateDisplay,
        //        AvgRatePerKg = avgRate,
        //        TotalAmount = totalAmount
        //    };
        //}

        public MilkPurchaseCalculateResponse CalculateMilk(MilkPurchaseCalculateRequest req)
        {
            var fatKey = Math.Round(req.Fat, 1);
            var clrKey = (int)Math.Floor(req.CLR);

            var key = (fatKey, clrKey);

            // SLAB NOT FOUND → SEND MESSAGE TO FE
            if (!SnfSlab.TryGetValue(key, out var snfPercent))
            {
                return new MilkPurchaseCalculateResponse
                {
                    IsSuccess = false,
                    Message = $"SNF slab not found for FAT={fatKey}, CLR={clrKey}. Please inform admin to add this slab."
                };
            }
            //var snfPercent = CalculateSnfFallback(fatKey, clrKey);


            // 1️⃣ Kg calculations
            var fatKg = Math.Round(req.Quantity * req.Fat / 100m, 2);
            var snfKg = Math.Round(req.Quantity * snfPercent / 100m, 2);


            // 2️⃣ Component values (Milky-compatible)
            var fatAmount = Math.Round(fatKg * req.Rate / 100m, 2);
            var snfAmount = Math.Round(snfKg * req.Rate / 200m, 2);

            // 3️⃣ Base amount
            var baseAmount = fatAmount + snfAmount;

            // 4️⃣ Calibrated Milky multiplier
            var totalAmount = Math.Round(
                baseAmount * 9.30m,  //baseAmount * 9.33m
                2,  
                MidpointRounding.AwayFromZero
            );

            return new MilkPurchaseCalculateResponse
            {
                IsSuccess = true,
                Message = "Milk calculation successful.",
                SNFPercent = snfPercent,
                FatKg = fatKg,
                SNFKg = snfKg,
                AvgRatePerKg = Math.Round(totalAmount / req.Quantity, 2),
                TotalAmount = totalAmount
            };
        }
        //        static readonly Dictionary<(decimal Fat, int Clr), decimal> SnfSlab = new()
        //{
        //    // FAT 4.5
        //    { (4.5m,27), 7.78m },
        //    { (4.5m,26), 7.54m },
        //    { (4.5m,25), 7.29m },
        //    { (4.5m,28), 8.00m },
        //    //{ (4.5m,23), 6.84m },
        //    { (4.5m,20), 6.03m },

        //    // FAT 4.4
        //    { (4.4m,27), 7.78m },
        //    { (4.4m,26), 7.51m },
        //    { (4.4m,23), 6.84m },
        //    { (4.4m,22), 6.74m },

        //    // FAT 4.0
        //    { (4.0m,27), 7.69m },
        //    { (4.0m,25), 6.94m },
        //    { (4.0m,22), 6.54m },
        //    { (4.0m,20), 5.94m },

        //    // FAT 3.5
        //    { (3.5m,20), 5.85m },
        //    { (3.5m,22), 6.06m },
        //    { (3.5m,25), 7.08m },
        //    { (3.5m,27), 7.60m },
        //    { (3.5m,30), 8.34m },

        //    // FAT 3.0
        //    { (3.0m,27), 7.48m },
        //    { (3.0m,26), 7.23m },
        //    { (3.0m,25), 7.20m },

        //    // FAT 2.7
        //    { (2.7m,26), 7.17m },
        //    { (2.7m,25), 6.92m },

        //    // FAT 2.5
        //    { (2.5m,20), 5.63m },
        //    { (2.5m,25), 6.89m },

        //    // Special / derived
        //    { (2.0m,20), 5.54m },
        //    { (3.2m,31), 8.66m },
        //    { (3.6m,19), 6.00m },
        //    { (4.2m,23), 6.93m },
        //    { (4.3m,23), 6.95m },
        //    { (4.6m,24), 7.20m },
        //    { (4.6m,22), 6.75m },
        //    { (4.7m,22), 6.65m },
        //    { (4.7m,21), 6.43m },
        //    { (4.8m,22), 6.82m },
        //    { (4.8m,26), 7.62m },
        //    { (5.0m,22), 6.84m },
        //    { (5.5m,24), 7.33m },
        //    { (5.8m,22), 6.92m },
        //    { (6.0m,9),  3.60m },
        //    { (6.4m,26), 8.08m },
        //    { (8.5m,37), 11.08m },
        //    { (6.0m, 27), 8.09m },
        //{ (6.0m, 28), 8.21m },
        //{ (6.0m, 30), 8.90m },
        //{ (6.0m, 35), 10.00m },
        //{ (5.7m, 29), 8.56m },
        //{ (5.7m, 26), 8.00m },
        //{ (5.2m, 26), 7.75m },
        //{ (5.2m, 29), 8.46m },
        //{ (5.2m, 30), 8.75m },
        //{ (5.1m, 28), 8.29m },
        //{ (4.9m, 29), 8.45m },
        //{ (4.9m, 23), 7.07m },
        //{ (4.8m, 23), 6.84m },
        //{ (4.8m, 29), 8.39m },
        //{ (4.7m, 24), 7.12m },
        //{ (4.7m, 25), 7.38m },
        //{ (4.7m, 29), 8.39m },
        //{ (4.3m, 26), 7.52m },
        //{ (4.1m, 21), 6.24m },
        //{ (4.1m, 30), 8.59m },
        //{ (3.8m, 36), 10.00m },
        //{ (3.7m, 21), 6.24m },
        //{ (3.7m, 32), 9.00m },
        //{ (3.2m, 19), 5.66m },
        //{ (3.2m, 20), 5.90m },
        //{ (2.8m, 24), 6.69m },
        //{ (2.8m, 30), 8.40m },
        //{ (2.0m, 30), 8.00m },
        //{ (2.0m, 41), 10.80m },
        //{ (6.0m, 25), 7.70m },   // case 195
        //{ (6.0m, 29), 8.66m },
        //{ (6.1m, 22), 7.23m },
        //{ (6.5m, 28), 8.56m },   // case 121
        //{ (6.9m, 29), 8.85m },   // case 36
        //{ (5.8m, 26), 7.90m },   // case 96
        //{ (5.7m, 22), 6.75m },   // case 77
        //{ (5.7m, 25), 7.55m },   // case 74
        //{ (5.5m, 22), 6.77m },   // case 131
        //{ (5.5m, 28), 8.30m },   // case 98
        //{ (5.3m, 26), 7.77m },   // case 46
        //{ (5.2m, 22), 6.70m },   // case 101
        //{ (5.2m, 23), 7.12m },   // case 106–110
        //{ (5.2m, 25), 7.60m },   // case 169–170
        //{ (5.1m, 23), 7.03m },   // case 190–191
        //{ (5.1m, 26), 7.74m },   // case 151
        //{ (5.1m, 27), 8.00m },   // case 111
        //{ (5.0m, 26), 7.63m },   // case 78–79
        //{ (5.0m, 28), 8.35m },   // case 105
        //{ (4.9m, 26), 7.83m },   // case 149–150
        ////{ (4.8m, 26), 7.90m },   // case 123
        //{ (4.8m, 30), 9.00m },   // case 186
        //{ (4.7m, 23), 7.00m },   // case 176–181
        //{ (4.7m, 26), 7.74m },   // case 158
        //{ (4.6m, 23), 6.83m },   // case 199
        //{ (4.6m, 25), 7.53m },   // case 122
        //{ (4.4m, 26), 7.70m },   // case 134
        //{ (4.4m, 20), 6.15m },   // case 156
        //{ (4.3m, 24), 7.06m },   // case 194
        //{ (4.2m, 24), 7.14m },   // case 112–113
        //{ (4.2m, 25), 7.22m },   // case 165
        //{ (4.1m, 23), 6.82m },   // case 41–42
        //{ (3.9m, 22), 6.40m },   // case 129–130
        //{ (3.8m, 25), 7.23m },   // case 126
        //{ (3.7m, 24), 7.00m },   // case 174
        //{ (3.3m, 21), 6.00m },   // case 135
        //{ (3.3m, 25), 7.14m },   // case 182
        //{ (2.8m, 25), 6.96m },   // case 136
        //{ (6.3m, 26), 7.90m },   // case 201
        //{ (6.3m, 27), 8.24m },   // case 258
        //{ (3.8m, 23), 6.76m },   // case 202
        //{ (3.8m, 22), 6.45m },   // case 286
        //{ (4.6m, 19), 6.02m },   // case 203
        //{ (4.6m, 26), 7.64m },   // case 279
        //{ (5.4m, 22), 6.71m },   // case 208
        //{ (5.4m, 24), 7.22m },   // case 211
        //{ (5.4m, 26), 7.81m },   // case 285
        //{ (5.3m, 21), 6.63m },   // case 209
        //{ (5.3m, 27), 7.95m },   // case 227
        //{ (5.3m, 28), 8.32m },   // case 261
        //{ (4.7m, 26), 7.73m },   // case 229 (diff qty → stable)
        //{ (4.7m, 28), 8.28m },   // case 293
        //{ (6.4m, 27), 8.17m },   // case 212
        //{ (6.2m, 29), 8.63m },   // case 213–214
        //{ (6.2m, 30), 8.88m },   // case 215
        //{ (5.1m, 25), 7.53m },   // case 275–278
        //{ (4.9m, 26), 7.72m },   // case 224–226
        //{ (4.8m, 29), 8.29m },   // case 255
        //{ (7.4m, 25), 8.00m },   // case 292
        //{ (7.0m, 23), 7.30m },   // case 87–88
        //{ (8.0m, 25), 8.14m },   // case 274
        //{ (8.7m, 26), 8.48m },   // case 271
        //{ (9.1m, 25), 8.22m },   // case 272
        //{ (2.3m, 20), 8.26m },   // case 265
        //// Derived from cases 1–92 (unique only)

        //// FAT 4.6
        //{ (4.6m, 24), 7.20m },   // case 1
        //{ (4.6m, 22), 6.89m },   // case 11

        //// FAT 5.5
        //{ (5.5m, 24), 7.33m },   // case 2 (confirming, keep)
        //{ (5.5m, 22), 6.92m },   // case 17

        //// FAT 4.8
        //{ (4.8m, 22), 7.11m },   // case 4
        //{ (4.8m, 23), 6.92m },   // case 9–10

        //// FAT 4.4
        //{ (4.4m, 23), 6.84m },   // case 5 (confirming)
        //{ (4.4m, 22), 6.74m },   // case 27–28 (confirming)

        //// FAT 4.0
        //{ (4.0m, 22), 6.54m },   // case 6 (confirming)

        //// FAT 4.2
        //{ (4.2m, 23), 6.93m },   // case 7 (confirming)

        //// FAT 5.0
        //{ (5.0m, 22), 6.84m },   // case 8 (confirming)

        //// FAT 4.3
        //{ (4.3m, 23), 6.95m },   // case 12 (confirming)
        //{ (4.3m, 24), 7.02m },   // case 16
        //{ (4.3m, 25), 7.24m },   // case 34

        //// FAT 4.7
        //{ (4.7m, 22), 6.65m },   // case 13–15 (confirming)
        //{ (4.7m, 21), 6.43m },   // case 25–26 (confirming)

        //// FAT 3.6
        //{ (3.6m, 19), 6.00m },   // case 19 (confirming)

        //// FAT 6.4
        //{ (6.4m, 26), 8.08m },   // case 20 (confirming)

        //// FAT 2.8
        //{ (2.8m, 30), 8.35m },   // case 21–22 (confirming)

        //// FAT 3.2
        //{ (3.2m, 31), 8.66m },   // case 23–24 (confirming)

        //// FAT 4.8 (higher CLR)
        ////{ (4.8m, 26), 7.63m },   // case 31–33

        //// FAT 5.8
        //{ (5.8m, 22), 6.92m },   // case 35 (confirming)

        //// FAT 5.4
        //{ (5.4m, 24), 7.21m },   // case 36
        //{ (5.4m, 21), 6.52m },   // case 39

        //// FAT 4.9
        //{ (4.9m, 22), 6.72m },   // case 38

        //// FAT 5.2
        //{ (5.2m, 21), 6.54m },   // case 41
        //{ (5.2m, 26), 7.73m },   // case 50

        //// FAT 6.2
        //{ (6.2m, 30), 8.88m },   // case 42–44

        //// FAT 5.1
        //{ (5.1m, 26), 7.70m },   // case 49
        //{ (5.1m, 27), 7.98m },   // case 65–68

        //// FAT 5.3
        //{ (5.3m, 25), 7.00m },   // case 51–52
        //{ (5.3m, 26), 7.80m },   // case 53
        //{ (5.3m, 27), 8.00m },   // case 54–58
        //{ (5.3m, 28), 8.30m },   // case 69–71
        //{ (5.3m, 29), 8.00m },   // case 72–75

        //// FAT 6.5
        //{ (6.5m, 29), 8.70m },   // case 76

        //// FAT 5.7
        //{ (5.7m, 27), 8.03m },   // case 80
        //{ (5.7m, 28), 8.28m },   // case 78–79

        //// FAT 4.0 (higher CLR)
        //{ (4.0m, 26), 7.43m },   // case 82

        //// FAT 2.2
        //{ (2.2m, 26), 7.00m },   // case 84

        //// FAT 1.8
        //{ (1.8m, 26), 7.00m },   // case 85

        //// FAT 0.9
        //{ (0.9m, 26), 7.00m },   // case 88

        //};
        static readonly Dictionary<(decimal Fat, int Clr), decimal> SnfSlab = new()
{
            // ================= FAT 1.0 ================= AI Based
// ⚠️ CLR 10–17 : Abnormal / Diluted milk (allow with warning)

//{ (1.0m, 10), 3.90m },
//{ (1.0m, 11), 4.10m },
//{ (1.0m, 12), 4.30m },
//{ (1.0m, 13), 4.50m },
//{ (1.0m, 14), 4.70m },
//{ (1.0m, 15), 4.90m },
//{ (1.0m, 16), 5.10m },
//{ (1.0m, 17), 5.30m },

//// ✅ Normal usable range starts here
//{ (1.0m, 18), 5.50m },
//{ (1.0m, 19), 5.70m },
//{ (1.0m, 20), 5.90m },
//{ (1.0m, 21), 6.10m },
//{ (1.0m, 22), 6.30m },
//{ (1.0m, 23), 6.50m },
//{ (1.0m, 24), 6.70m },
//{ (1.0m, 25), 6.90m },
//{ (1.0m, 26), 7.05m },
//{ (1.0m, 27), 7.25m },
//{ (1.0m, 28), 7.45m },
//{ (1.0m, 29), 7.65m },
//{ (1.0m, 30), 7.85m },
//{ (1.0m, 31), 8.05m },
//{ (1.0m, 32), 8.25m },
//{ (1.0m, 33), 8.45m },
//{ (1.0m, 34), 8.65m },
//{ (1.0m, 35), 8.85m },
//{ (1.0m, 36), 9.05m },
//{ (1.0m, 37), 9.25m },
//{ (1.0m, 38), 9.45m },
//{ (1.0m, 39), 9.65m },
//{ (1.0m, 40), 9.85m },
//{ (1.0m, 41), 10.05m },
////================= FAT 1.1 =================
//{ (1.1m,10), 3.92m }, { (1.1m,11), 4.12m }, { (1.1m,12), 4.32m },
//{ (1.1m,13), 4.52m }, { (1.1m,14), 4.72m }, { (1.1m,15), 4.92m },
//{ (1.1m,16), 5.12m }, { (1.1m,17), 5.32m }, { (1.1m,18), 5.52m },
//{ (1.1m,19), 5.72m }, { (1.1m,20), 5.92m }, { (1.1m,21), 6.12m },
//{ (1.1m,22), 6.32m }, { (1.1m,23), 6.52m }, { (1.1m,24), 6.72m },
//{ (1.1m,25), 6.92m }, { (1.1m,26), 7.07m }, { (1.1m,27), 7.27m },
//{ (1.1m,28), 7.47m }, { (1.1m,29), 7.67m }, { (1.1m,30), 7.87m },
//{ (1.1m,31), 8.07m }, { (1.1m,32), 8.27m }, { (1.1m,33), 8.47m },
//{ (1.1m,34), 8.67m }, { (1.1m,35), 8.87m }, { (1.1m,36), 9.07m },
//{ (1.1m,37), 9.27m }, { (1.1m,38), 9.47m }, { (1.1m,39), 9.67m },
//{ (1.1m,40), 9.87m }, { (1.1m,41),10.07m },
////================= FAT 1.3 =================
//{ (1.3m,10), 3.96m }, { (1.3m,11), 4.16m }, { (1.3m,12), 4.36m },
//{ (1.3m,13), 4.56m }, { (1.3m,14), 4.76m }, { (1.3m,15), 4.96m },
//{ (1.3m,16), 5.16m }, { (1.3m,17), 5.36m }, { (1.3m,18), 5.56m },
//{ (1.3m,19), 5.76m }, { (1.3m,20), 5.96m }, { (1.3m,21), 6.16m },
//{ (1.3m,22), 6.36m }, { (1.3m,23), 6.56m }, { (1.3m,24), 6.76m },
//{ (1.3m,25), 6.96m }, { (1.3m,26), 7.11m }, { (1.3m,27), 7.31m },
//{ (1.3m,28), 7.51m }, { (1.3m,29), 7.71m }, { (1.3m,30), 7.91m },
//{ (1.3m,31), 8.11m }, { (1.3m,32), 8.31m }, { (1.3m,33), 8.51m },
//{ (1.3m,34), 8.71m }, { (1.3m,35), 8.91m }, { (1.3m,36), 9.11m },
//{ (1.3m,37), 9.31m }, { (1.3m,38), 9.51m }, { (1.3m,39), 9.71m },
//{ (1.3m,40), 9.91m }, { (1.3m,41),10.11m },

////================= FAT 1.5 =================
//{ (1.5m,10), 4.00m }, { (1.5m,11), 4.20m }, { (1.5m,12), 4.40m },
//{ (1.5m,13), 4.60m }, { (1.5m,14), 4.80m }, { (1.5m,15), 5.00m },
//{ (1.5m,16), 5.20m }, { (1.5m,17), 5.40m }, { (1.5m,18), 5.60m },
//{ (1.5m,19), 5.80m }, { (1.5m,20), 6.00m }, { (1.5m,21), 6.20m },
//{ (1.5m,22), 6.40m }, { (1.5m,23), 6.60m }, { (1.5m,24), 6.80m },
//{ (1.5m,25), 7.00m }, { (1.5m,26), 7.15m }, { (1.5m,27), 7.35m },
//{ (1.5m,28), 7.55m }, { (1.5m,29), 7.75m }, { (1.5m,30), 7.95m },
//{ (1.5m,31), 8.15m }, { (1.5m,32), 8.35m }, { (1.5m,33), 8.55m },
//{ (1.5m,34), 8.75m }, { (1.5m,35), 8.95m }, { (1.5m,36), 9.15m },
//{ (1.5m,37), 9.35m }, { (1.5m,38), 9.55m }, { (1.5m,39), 9.75m },
//{ (1.5m,40), 9.95m }, { (1.5m,41),10.15m },

////================= FAT 1.7 =================
//{ (1.7m,10), 4.04m }, { (1.7m,11), 4.24m }, { (1.7m,12), 4.44m },
//{ (1.7m,13), 4.64m }, { (1.7m,14), 4.84m }, { (1.7m,15), 5.04m },
//{ (1.7m,16), 5.24m }, { (1.7m,17), 5.44m }, { (1.7m,18), 5.64m },
//{ (1.7m,19), 5.84m }, { (1.7m,20), 6.04m }, { (1.7m,21), 6.24m },
//{ (1.7m,22), 6.44m }, { (1.7m,23), 6.64m }, { (1.7m,24), 6.84m },
//{ (1.7m,25), 7.04m }, { (1.7m,26), 7.19m }, { (1.7m,27), 7.39m },
//{ (1.7m,28), 7.59m }, { (1.7m,29), 7.79m }, { (1.7m,30), 7.99m },
//{ (1.7m,31), 8.19m }, { (1.7m,32), 8.39m }, { (1.7m,33), 8.59m },
//{ (1.7m,34), 8.79m }, { (1.7m,35), 8.99m }, { (1.7m,36), 9.19m },
//{ (1.7m,37), 9.39m }, { (1.7m,38), 9.59m }, { (1.7m,39), 9.79m },
//{ (1.7m,40), 9.99m }, { (1.7m,41),10.19m },

////================= FAT 1.9 =================
//{ (1.9m,10), 4.08m }, { (1.9m,11), 4.28m }, { (1.9m,12), 4.48m },
//{ (1.9m,13), 4.68m }, { (1.9m,14), 4.88m }, { (1.9m,15), 5.08m },
//{ (1.9m,16), 5.28m }, { (1.9m,17), 5.48m }, { (1.9m,18), 5.68m },
//{ (1.9m,19), 5.88m }, { (1.9m,20), 6.08m }, { (1.9m,21), 6.28m },
//{ (1.9m,22), 6.48m }, { (1.9m,23), 6.68m }, { (1.9m,24), 6.88m },
//{ (1.9m,25), 7.08m }, { (1.9m,26), 7.23m }, { (1.9m,27), 7.43m },
//{ (1.9m,28), 7.63m }, { (1.9m,29), 7.83m }, { (1.9m,30), 8.03m },
//{ (1.9m,31), 8.23m }, { (1.9m,32), 8.43m }, { (1.9m,33), 8.63m },
//{ (1.9m,34), 8.83m }, { (1.9m,35), 9.03m }, { (1.9m,36), 9.23m },
//{ (1.9m,37), 9.43m }, { (1.9m,38), 9.63m }, { (1.9m,39), 9.83m },
//{ (1.9m,40),10.03m }, { (1.9m,41),10.23m },

////================= FAT 1.2 =================
//{ (1.2m,10), 3.94m }, { (1.2m,11), 4.14m }, { (1.2m,12), 4.34m },
//{ (1.2m,13), 4.54m }, { (1.2m,14), 4.74m }, { (1.2m,15), 4.94m },
//{ (1.2m,16), 5.14m }, { (1.2m,17), 5.34m }, { (1.2m,18), 5.54m },
//{ (1.2m,19), 5.74m }, { (1.2m,20), 5.94m }, { (1.2m,21), 6.14m },
//{ (1.2m,22), 6.34m }, { (1.2m,23), 6.54m }, { (1.2m,24), 6.74m },
//{ (1.2m,25), 6.94m }, { (1.2m,26), 7.09m }, { (1.2m,27), 7.29m },
//{ (1.2m,28), 7.49m }, { (1.2m,29), 7.69m }, { (1.2m,30), 7.89m },
//{ (1.2m,31), 8.09m }, { (1.2m,32), 8.29m }, { (1.2m,33), 8.49m },
//{ (1.2m,34), 8.69m }, { (1.2m,35), 8.89m }, { (1.2m,36), 9.09m },
//{ (1.2m,37), 9.29m }, { (1.2m,38), 9.49m }, { (1.2m,39), 9.69m },
//{ (1.2m,40), 9.89m }, { (1.2m,41),10.09m },

////================= FAT 1.4 =================
//{ (1.4m,10), 3.98m }, { (1.4m,11), 4.18m }, { (1.4m,12), 4.38m },
//{ (1.4m,13), 4.58m }, { (1.4m,14), 4.78m }, { (1.4m,15), 4.98m },
//{ (1.4m,16), 5.18m }, { (1.4m,17), 5.38m }, { (1.4m,18), 5.58m },
//{ (1.4m,19), 5.78m }, { (1.4m,20), 5.98m }, { (1.4m,21), 6.18m },
//{ (1.4m,22), 6.38m }, { (1.4m,23), 6.58m }, { (1.4m,24), 6.78m },
//{ (1.4m,25), 6.98m }, { (1.4m,26), 7.13m }, { (1.4m,27), 7.33m },
//{ (1.4m,28), 7.53m }, { (1.4m,29), 7.73m }, { (1.4m,30), 7.93m },
//{ (1.4m,31), 8.13m }, { (1.4m,32), 8.33m }, { (1.4m,33), 8.53m },
//{ (1.4m,34), 8.73m }, { (1.4m,35), 8.93m }, { (1.4m,36), 9.13m },
//{ (1.4m,37), 9.33m }, { (1.4m,38), 9.53m }, { (1.4m,39), 9.73m },
//{ (1.4m,40), 9.93m }, { (1.4m,41),10.13m },

////================= FAT 1.6 =================
//{ (1.6m,10), 4.02m }, { (1.6m,11), 4.22m }, { (1.6m,12), 4.42m },
//{ (1.6m,13), 4.62m }, { (1.6m,14), 4.82m }, { (1.6m,15), 5.02m },
//{ (1.6m,16), 5.22m }, { (1.6m,17), 5.42m }, { (1.6m,18), 5.62m },
//{ (1.6m,19), 5.82m }, { (1.6m,20), 6.02m }, { (1.6m,21), 6.22m },
//{ (1.6m,22), 6.42m }, { (1.6m,23), 6.62m }, { (1.6m,24), 6.82m },
//{ (1.6m,25), 7.02m }, { (1.6m,26), 7.17m }, { (1.6m,27), 7.37m },
//{ (1.6m,28), 7.57m }, { (1.6m,29), 7.77m }, { (1.6m,30), 7.97m },
//{ (1.6m,31), 8.17m }, { (1.6m,32), 8.37m }, { (1.6m,33), 8.57m },
//{ (1.6m,34), 8.77m }, { (1.6m,35), 8.97m }, { (1.6m,36), 9.17m },
//{ (1.6m,37), 9.37m }, { (1.6m,38), 9.57m }, { (1.6m,39), 9.77m },
//{ (1.6m,40), 9.97m }, { (1.6m,41),10.17m },

// APP Based
    // ================= FAT 0–2 =================
    { (0.9m, 26), 7.00m },
    { (1.8m, 26), 7.00m },
    { (2.0m, 20), 5.54m },
    { (2.0m, 30), 8.00m },
    { (2.0m, 41), 10.80m },
    { (2.2m, 26), 7.00m },
    { (2.3m, 20), 5.80m },
    { (2.5m, 20), 5.63m },
    { (2.5m, 25), 6.89m },
    { (2.7m, 25), 6.92m },
    { (2.7m, 26), 7.17m },
    { (2.8m, 24), 6.69m },
    { (2.8m, 25), 6.96m },
    { (2.8m, 30), 8.40m },

    // ================= FAT 3.x =================
    { (3.0m, 25), 7.20m },
    { (3.0m, 26), 7.23m },
    { (3.0m, 27), 7.48m },
    { (3.2m, 19), 5.66m },
    { (3.2m, 20), 5.90m },
    { (3.2m, 31), 8.66m },
    { (3.3m, 21), 6.00m },
    { (3.3m, 25), 7.14m },
    { (3.5m, 20), 5.85m },
    { (3.5m, 22), 6.06m },
    { (3.5m, 25), 7.08m },
    { (3.5m, 27), 7.60m },
    { (3.5m, 30), 8.34m },
    { (3.6m, 19), 6.00m },
    { (3.7m, 21), 6.24m },
    { (3.7m, 24), 7.00m },
    { (3.7m, 32), 9.00m },
    { (3.8m, 22), 6.45m },
    { (3.8m, 23), 6.76m },
    { (3.8m, 25), 7.23m },
    { (3.8m, 36), 10.00m },
    { (3.9m, 22), 6.40m },

    // ================= FAT 4.x =================
    { (4.0m, 20), 5.94m },
    { (4.0m, 22), 6.54m },
    { (4.0m, 25), 6.94m },
    { (4.0m, 26), 7.43m },
    { (4.0m, 27), 7.69m },
    { (4.1m, 21), 6.24m },
    { (4.1m, 23), 6.82m },
    { (4.1m, 30), 8.59m },
    { (4.2m, 23), 6.93m },
    { (4.2m, 24), 7.14m },
    { (4.2m, 25), 7.22m },
    { (4.3m, 23), 6.95m },
    { (4.3m, 24), 7.06m },
    { (4.3m, 25), 7.24m },
    { (4.3m, 26), 7.52m },
    { (4.4m, 20), 6.15m },
    { (4.4m, 22), 6.74m },
    { (4.4m, 23), 6.84m },
    { (4.4m, 26), 7.70m },
    { (4.4m, 27), 7.78m },
    { (4.5m, 20), 6.03m },
    { (4.5m, 25), 7.29m },
    { (4.5m, 26), 7.54m },
    { (4.5m, 27), 7.78m },
    { (4.5m, 28), 8.00m },
    { (4.5m, 23), 6.95m },
    { (4.6m, 22), 6.75m },
    { (4.6m, 23), 6.83m },
    { (4.6m, 24), 7.20m },
    { (4.6m, 25), 7.53m },
    { (4.6m, 26), 7.64m },
    { (4.7m, 21), 6.43m },
    { (4.7m, 22), 6.65m },
    { (4.7m, 23), 7.00m },
    { (4.7m, 24), 7.12m },
    { (4.7m, 25), 7.38m },
    { (4.7m, 26), 7.74m },
    { (4.7m, 28), 8.28m },
    { (4.7m, 29), 8.39m },
    { (4.8m, 22), 6.82m },
    { (4.8m, 23), 6.84m },
    { (4.8m, 26), 7.62m },
    { (4.8m, 29), 8.29m },
    { (4.8m, 30), 9.00m },
    { (4.9m, 22), 6.72m },
    { (4.9m, 23), 7.07m },
    { (4.9m, 26), 7.83m },
    { (4.9m, 29), 8.45m },

    // ================= FAT 5.x =================
    { (5.0m, 22), 6.84m },
    { (5.0m, 26), 7.63m },
    { (5.0m, 28), 8.35m },
    { (5.1m, 23), 7.03m },
    { (5.1m, 25), 7.53m },
    { (5.1m, 26), 7.74m },
    { (5.1m, 27), 8.00m },
    { (5.1m, 28), 8.29m },
    { (5.2m, 22), 6.70m },
    { (5.2m, 23), 7.12m },
    { (5.2m, 25), 7.60m },
    { (5.2m, 26), 7.75m },
    { (5.2m, 29), 8.46m },
    { (5.2m, 30), 8.75m },
    { (5.2m, 41), 10.80m },
    { (5.3m, 21), 6.63m },
    { (5.3m, 25), 7.00m },
    { (5.3m, 26), 7.77m },
    { (5.3m, 27), 8.00m },
    { (5.3m, 28), 8.30m },
    { (5.3m, 29), 8.56m },
    { (5.4m, 21), 6.52m },
    { (5.4m, 22), 6.71m },
    { (5.4m, 24), 7.22m },
    { (5.4m, 26), 7.81m },
    { (5.5m, 22), 6.77m },
    { (5.5m, 24), 7.33m },
    { (5.5m, 27), 8.00m },
    { (5.5m, 28), 8.30m },
    { (5.6m, 29), 8.48m },
    { (5.7m, 22), 6.75m },
    { (5.7m, 25), 7.55m },
    { (5.7m, 26), 8.00m },
    { (5.7m, 27), 8.03m },
    { (5.7m, 28), 8.28m },
    { (5.7m, 29), 8.56m },
    { (5.8m, 22), 6.92m },
    { (5.8m, 26), 7.90m },
    { (5.8m, 27), 8.05m },

    // ================= FAT 6+ =================
    { (6.0m, 9), 3.60m },
    { (6.0m, 25), 7.70m },
    { (6.0m, 27), 8.09m },
    { (6.0m, 28), 8.21m },
    { (6.0m, 29), 8.66m },
    { (6.0m, 30), 8.90m },
    { (6.0m, 35), 10.00m },
    { (6.1m, 22), 7.23m },
    { (6.1m, 26), 7.88m },
    { (6.1m, 27), 8.12m },
    { (6.2m, 29), 8.63m },
    { (6.2m, 30), 8.88m },
    { (6.3m, 26), 7.90m },
    { (6.3m, 27), 8.24m },
    { (6.4m, 26), 8.08m },
    { (6.4m, 27), 8.17m },
    { (6.5m, 28), 8.56m },
    { (6.5m, 29), 8.70m },
    { (6.9m, 29), 8.85m },
    { (7.0m, 23), 7.30m },
    { (7.4m, 25), 8.00m },
    { (8.0m, 25), 8.14m },
    { (8.5m, 37), 11.08m },
    { (8.7m, 26), 8.48m },
    { (9.1m, 25), 8.22m },
    // FAT 3.0
{ (3.0m, 22), 6.60m },   // between (3.0,21≈6.4) & (3.0,25=7.20)

// FAT 5.5
{ (5.5m, 26), 7.90m },   // between (5.5,24=7.33) & (5.5,27=8.00)
//{ (5.5m, 28), 8.30m },   // matches progression with (5.5,27=8.00)

// FAT 5.4
//{ (5.4m, 22), 6.71m },   // matches pattern with nearby
{ (5.4m, 25), 7.55m },   // interpolated between (24=7.22) & (26=7.81)
{ (5.4m, 28), 8.35m },   // smooth continuation after CLR 26

// FAT 1.3
{ (1.3m, 26), 7.11m },   // aligns with your FAT 1.x AI slabs
{ (1.3m, 27), 7.31m },

// FAT 5.6
{ (5.6m, 27), 8.20m },   // between (5.6,26≈8.00) & (5.6,29=8.48)

// FAT 6.0
{ (6.0m, 26), 7.95m },

};



        // STEP 2: CONFIRM & SAVE
        public async Task ConfirmMilkPurchaseAsync(
            MilkPurchaseConfirmRequest request)
        {
            await _repo.SaveMilkPurchaseAsync(request);
        }

        public OtherMaterialPurchaseCalculateResponse CalculateOtherMaterial(
        OtherMaterialPurchaseCalculateRequest request)
        {
            var amount = request.Quantity * request.RatePerUnit;

            return new OtherMaterialPurchaseCalculateResponse
            {
                Amount = Math.Round(amount, 2)
            };
        }

        public async Task ConfirmOtherMaterialPurchaseAsync(
            OtherMaterialPurchaseConfirmRequest request)
        {
            await _repo.SaveOtherMaterialPurchaseAsync(request);
        }

        public async Task<PagedResult<PurchaseListDto>> GetPurchasesAsync(
    int pageNumber,
    int pageSize,
    string? rawMaterialType,
    DateTime? fromDate,
    DateTime? toDate)
        {
            var query = _repo.QueryPurchases();

            if (!string.IsNullOrWhiteSpace(rawMaterialType))
                query = query.Where(x => x.RawMaterialType == rawMaterialType);

            if (fromDate.HasValue)
                query = query.Where(x => x.PurchaseDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(x => x.PurchaseDate <= toDate.Value);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.PurchaseDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new PurchaseListDto
                {
                    PurchaseId = x.PurchaseId,
                    PurchaseDate = x.PurchaseDate,
                    RawMaterialType = x.RawMaterialType,
                    Quantity = x.Quantity,
                    Unit = x.Unit,
                    RatePerUnit = x.RatePerUnit,
                    Amount = x.Amount
                })
                .ToListAsync();

            return new PagedResult<PurchaseListDto>
            {
                Items = items,
                TotalRecords = total
            };
        }
    }
}
