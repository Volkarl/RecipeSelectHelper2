﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using RecipeSelectHelper.Model.SortingMethods;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "BoughtProduct")]
    public class BoughtProduct : IBoughtProduct
    {

        [DataMember]
        public Product CorrespondingProduct { get; set; }
        [DataMember]
        public ExpirationInfo ExpirationData { get; set; }
        [DataMember]
        public uint Amount { get; set; }

        private ValueInformation _ownValue = new ValueInformation();
        public ValueInformation OwnValue => _ownValue ?? (_ownValue = new ValueInformation()); //Needed for deserialization
        // The BoughtProduct doesn't aggregate the value of the correspondingProduct, to avoid Product values 
        // double-dipping, as Ingredients do aggregate correspondingProduct. Intuitively this also makes sense, 
        // because regardless of whether something is a vegetable or not, if it's a day out from expiration, 
        // it should have priority for being used for recipes. 

        public void Reset() => OwnValue.Reset();

        private BoughtProduct() { }

        public BoughtProduct(Product correspondingProduct, uint amount, ExpirationInfo expirationData = null)
        {
            if(correspondingProduct == null) throw new ArgumentException("No product selected");
            CorrespondingProduct = correspondingProduct;
            Amount = amount;
            ExpirationData = expirationData ?? new ExpirationInfo();
        }
    }
}