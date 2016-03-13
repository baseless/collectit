﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CollectIt.Web.ChannelService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RichChannel", Namespace="http://schemas.datacontract.org/2004/07/CollectIt.WCF.Contracts")]
    [System.SerializableAttribute()]
    public partial class RichChannel : CollectIt.Web.ChannelService.Channel {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] FiltersField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsSubscribingField;
        
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
        public bool IsSubscribing {
            get {
                return this.IsSubscribingField;
            }
            set {
                if ((this.IsSubscribingField.Equals(value) != true)) {
                    this.IsSubscribingField = value;
                    this.RaisePropertyChanged("IsSubscribing");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Channel", Namespace="http://schemas.datacontract.org/2004/07/CollectIt.WCF.Contracts")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CollectIt.Web.ChannelService.RichChannel))]
    public partial class Channel : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid ChannelIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
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
        public System.Guid ChannelId {
            get {
                return this.ChannelIdField;
            }
            set {
                if ((this.ChannelIdField.Equals(value) != true)) {
                    this.ChannelIdField = value;
                    this.RaisePropertyChanged("ChannelId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ChannelService.IChannelService")]
    public interface IChannelService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChannelService/AllChannels", ReplyAction="http://tempuri.org/IChannelService/AllChannelsResponse")]
        CollectIt.Web.ChannelService.RichChannel[] AllChannels(string userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChannelService/AllChannels", ReplyAction="http://tempuri.org/IChannelService/AllChannelsResponse")]
        System.Threading.Tasks.Task<CollectIt.Web.ChannelService.RichChannel[]> AllChannelsAsync(string userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChannelService/SubscribedChannels", ReplyAction="http://tempuri.org/IChannelService/SubscribedChannelsResponse")]
        CollectIt.Web.ChannelService.Channel[] SubscribedChannels(string userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChannelService/SubscribedChannels", ReplyAction="http://tempuri.org/IChannelService/SubscribedChannelsResponse")]
        System.Threading.Tasks.Task<CollectIt.Web.ChannelService.Channel[]> SubscribedChannelsAsync(string userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChannelService/Subscribe", ReplyAction="http://tempuri.org/IChannelService/SubscribeResponse")]
        void Subscribe(string userId, CollectIt.Web.ChannelService.Channel channel, string[] filters);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChannelService/Subscribe", ReplyAction="http://tempuri.org/IChannelService/SubscribeResponse")]
        System.Threading.Tasks.Task SubscribeAsync(string userId, CollectIt.Web.ChannelService.Channel channel, string[] filters);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChannelService/Unsubscribe", ReplyAction="http://tempuri.org/IChannelService/UnsubscribeResponse")]
        void Unsubscribe(string userId, CollectIt.Web.ChannelService.Channel channel);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChannelService/Unsubscribe", ReplyAction="http://tempuri.org/IChannelService/UnsubscribeResponse")]
        System.Threading.Tasks.Task UnsubscribeAsync(string userId, CollectIt.Web.ChannelService.Channel channel);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IChannelServiceChannel : CollectIt.Web.ChannelService.IChannelService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ChannelServiceClient : System.ServiceModel.ClientBase<CollectIt.Web.ChannelService.IChannelService>, CollectIt.Web.ChannelService.IChannelService {
        
        public ChannelServiceClient() {
        }
        
        public ChannelServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ChannelServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ChannelServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ChannelServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public CollectIt.Web.ChannelService.RichChannel[] AllChannels(string userId) {
            return base.Channel.AllChannels(userId);
        }
        
        public System.Threading.Tasks.Task<CollectIt.Web.ChannelService.RichChannel[]> AllChannelsAsync(string userId) {
            return base.Channel.AllChannelsAsync(userId);
        }
        
        public CollectIt.Web.ChannelService.Channel[] SubscribedChannels(string userId) {
            return base.Channel.SubscribedChannels(userId);
        }
        
        public System.Threading.Tasks.Task<CollectIt.Web.ChannelService.Channel[]> SubscribedChannelsAsync(string userId) {
            return base.Channel.SubscribedChannelsAsync(userId);
        }
        
        public void Subscribe(string userId, CollectIt.Web.ChannelService.Channel channel, string[] filters) {
            base.Channel.Subscribe(userId, channel, filters);
        }
        
        public System.Threading.Tasks.Task SubscribeAsync(string userId, CollectIt.Web.ChannelService.Channel channel, string[] filters) {
            return base.Channel.SubscribeAsync(userId, channel, filters);
        }
        
        public void Unsubscribe(string userId, CollectIt.Web.ChannelService.Channel channel) {
            base.Channel.Unsubscribe(userId, channel);
        }
        
        public System.Threading.Tasks.Task UnsubscribeAsync(string userId, CollectIt.Web.ChannelService.Channel channel) {
            return base.Channel.UnsubscribeAsync(userId, channel);
        }
    }
}
