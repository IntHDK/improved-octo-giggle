using improved_octo_giggle_DatacenterInterface;

namespace improved_octo_giggle_server.Services.DataCenter_LocalModule
{
    public partial class DataCenterLocal : DataCenter
    {
        public DataBroadcaster Broadcaster { get; set; }
        public class DataBroadcaster : DatacenterBroadcaster
        {
            public DataBroadcaster()
            {
            }

            private class ListenerInfo
            {
                public string ID { get; set; } = string.Empty;
                public ResourceType ResourceType { get; set; }
                public string ResourceID { get; set; } = string.Empty;
                public Callback? Callback { get; set; }
            }
            private Dictionary<string, ListenerInfo> _listeners = new Dictionary<string, ListenerInfo>();
            private Dictionary<ResourceType, Dictionary<string, HashSet<ListenerInfo>>> _listenersbyresource = new Dictionary<ResourceType, Dictionary<string, HashSet<ListenerInfo>>>();

            public void Dismiss(string ListeningID)
            {
                var listener = GetListenerInfo(ListeningID);
                if (listener != null)
                {
                    _listeners.Remove(ListeningID);
                }
            }

            public string Listen(ResourceType ResourceType, string ResourceID, Callback callback)
            {
                throw new NotImplementedException();
            }

            private ListenerInfo? GetListenerInfo(string ListeningID)
            {
                ListenerInfo? listenerInfo;
                _listeners.TryGetValue(ListeningID, out listenerInfo);
                return listenerInfo;
            }
            private HashSet<ListenerInfo> GetListenerInfoByResourceID(ResourceType ResourceType, string ResourceID)
            {
                Dictionary<string, HashSet<ListenerInfo>>? resourcelistenerpool;
                HashSet<ListenerInfo>? listenersInfo;
                _listenersbyresource.TryGetValue(ResourceType, out resourcelistenerpool);
                if (resourcelistenerpool != null)
                {
                    if (resourcelistenerpool.TryGetValue(ResourceID, out listenersInfo))
                    {
                        return listenersInfo;
                    }
                    else
                    {
                        return new HashSet<ListenerInfo>();
                    }
                }
                else
                {
                    return new HashSet<ListenerInfo>();
                }
            }
            private bool AddListener(string ListeningID, ListenerInfo listenerInfo)
            {
                if (listenerInfo.ID == "" || listenerInfo.ResourceID == "")
                {
                    return false;
                }

                if (!_listenersbyresource.TryGetValue(listenerInfo.ResourceType, out _))
                {
                    if(!_listenersbyresource.TryAdd(listenerInfo.ResourceType, []))
                    {
                        return false;
                    }
                }

                return _listeners.TryAdd(ListeningID, listenerInfo);
            }

        }
        
    }
}
