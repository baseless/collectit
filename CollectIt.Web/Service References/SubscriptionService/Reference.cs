﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CollectIt.Web.SubscriptionService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SubscriptionContract", Namespace="http://schemas.datacontract.org/2004/07/CollectIt.WCF.Contracts")]
    [System.SerializableAttribute()]
    public partial class SubscriptionContract : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] FiltersField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PartitionKeyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RowKeyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime SubscribedDateField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Filters {
            get {
                return this.FiltersField;
            }
            set {
                if ((object.ReferenceEquals(this.FiltersField, value) != true)) {
                    this.FiltersField = value;
                    this.RaisePropertyChanged("Filters");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PartitionKey {
            get {
                return this.PartitionKeyField;
            }
            set {
                if ((object.ReferenceEquals(this.PartitionKeyField, value) != true)) {
                    this.PartitionKeyField = value;
                    this.RaisePropertyChanged("PartitionKey");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RowKey {
            get {
                return this.RowKeyField;
            }
            set {
                if ((object.ReferenceEquals(this.RowKeyField, value) != true)) {
                    this.RowKeyField = value;
                    this.RaisePropertyChanged("RowKey");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime SubscribedDate {
            get {
                return this.SubscribedDateField;
            }
            set {
                if ((this.SubscribedDateField.Equals(value) != true)) {
                    this.SubscribedDateField = value;
                    this.RaisePropertyChanged("SubscribedDate");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SubscriptionService.ISubscriptionService")]
    public interface ISubscriptionService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISubscriptionService/Get", ReplyAction="http://tempuri.org/ISubscriptionService/GetResponse")]
        CollectIt.Web.SubscriptionService.SubscriptionContract Get(string userId, string channelPartitionKey, string channelRowKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISubscriptionService/Get", ReplyAction="http://tempuri.org/ISubscriptionService/GetResponse")]
        System.Threading.Tasks.Task<CollectIt.Web.SubscriptionService.SubscriptionContract> GetAsync(string userId, string channelPartitionKey, string channelRowKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISubscriptionService/Subscribe", ReplyAction="http://tempuri.org/ISubscriptionService/SubscribeResponse")]
        void Subscribe(string userId, string channelPartitionKey, string channelRowKey, string[] filters);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISubscriptionService/Subscribe", ReplyAction="http://tempuri.org/ISubscriptionService/SubscribeResponse")]
        System.Threading.Tasks.Task SubscribeAsync(string userId, string channelPartitionKey, string channelRowKey, string[] filters);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISubscriptionService/Unsubscribe", ReplyAction="http://tempuri.org/ISubscriptionService/UnsubscribeResponse")]
        void Unsubscribe(string channelPartitionKey, string channelRowKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISubscriptionService/Unsubscribe", ReplyAction="http://tempuri.org/ISubscriptionService/UnsubscribeResponse")]
        System.Threading.Tasks.Task UnsubscribeAsync(string channelPartitionKey, string channelRowKey);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISubscriptionServiceChannel : CollectIt.Web.SubscriptionService.ISubscriptionService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SubscriptionServiceClient : System.ServiceModel.ClientBase<CollectIt.Web.SubscriptionService.ISubscriptionService>, CollectIt.Web.SubscriptionService.ISubscriptionService {
        
        public SubscriptionServiceClient() {
        }
        
        public SubscriptionServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SubscriptionServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SubscriptionServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SubscriptionServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public CollectIt.Web.SubscriptionService.SubscriptionContract Get(string userId, string channelPartitionKey, string channelRowKey) {
            return base.Channel.Get(userId, channelPartitionKey, channelRowKey);
        }
        
        public System.Threading.Tasks.Task<CollectIt.Web.SubscriptionService.SubscriptionContract> GetAsync(string userId, string channelPartitionKey, string channelRowKey) {
            return base.Channel.GetAsync(userId, channelPartitionKey, channelRowKey);
        }
        
        public void Subscribe(string userId, string channelPartitionKey, string channelRowKey, string[] filters) {
            base.Channel.Subscribe(userId, channelPartitionKey, channelRowKey, filters);
        }
        
        public System.Threading.Tasks.Task SubscribeAsync(string userId, string channelPartitionKey, string channelRowKey, string[] filters) {
            return base.Channel.SubscribeAsync(userId, channelPartitionKey, channelRowKey, filters);
        }
        
        public void Unsubscribe(string channelPartitionKey, string channelRowKey) {
            base.Channel.Unsubscribe(channelPartitionKey, channelRowKey);
        }
        
        public System.Threading.Tasks.Task UnsubscribeAsync(string channelPartitionKey, string channelRowKey) {
            return base.Channel.UnsubscribeAsync(channelPartitionKey, channelRowKey);
        }
    }
}
