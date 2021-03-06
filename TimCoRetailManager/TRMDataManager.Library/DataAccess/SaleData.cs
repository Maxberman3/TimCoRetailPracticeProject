﻿using System;
using System.Collections.Generic;
using System.Linq;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel sale, string cashierId)
        {
            ProductData productData = new ProductData();
            List<SaleDetailDbModel> details = new List<SaleDetailDbModel>();
            //Create a model for insertion in DB for each sale detail model that was posted to the api 
            foreach (SaleDetailModel item in sale.saleDetails)
            {
                ProductModel productInfo = productData.GetProductById(item.Id);
                if (productInfo == null)
                {
                    throw new System.Exception($"The product with id {item.Id} could not be found in database");
                }
                decimal tax = 0;
                decimal purchasePrice = productInfo.RetailPrice * item.Quantity;
                if (productInfo.IsTaxable)
                {
                    tax = purchasePrice * Convert.ToDecimal(ConfigHelper.GetTaxRate());
                }
                details.Add(new SaleDetailDbModel
                {
                    ProductId = item.Id,
                    Quantity = item.Quantity,
                    PurchasePrice = purchasePrice,
                    Tax = tax,
                });
            }
            //create the sale model
            SaleDbModel saleDbModel = new SaleDbModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId,
            };
            saleDbModel.Total = saleDbModel.SubTotal + saleDbModel.Tax;
            using (SqlDataAccess dataAccess = new SqlDataAccess())
            {
                try
                {
                    dataAccess.StartTransaction("TRMData");
                    dataAccess.SaveDataInTransaction("dbo.spSale_Insert", saleDbModel);

                    saleDbModel.Id = dataAccess.LoadDataInTransaction<int, dynamic>("dbo.spLookUp", new { CashierId = cashierId, saleDbModel.SaleDate }).FirstOrDefault();
                    //Finish filling in the sale detail models
                    foreach (SaleDetailDbModel item in details)
                    {
                        item.SaleId = saleDbModel.Id;
                        dataAccess.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                    };
                    dataAccess.CommitTransaction();
                }
                catch (Exception e)
                {

                    dataAccess.RollBackTransaction();
                    throw;
                }
            }


        }
    }
}
