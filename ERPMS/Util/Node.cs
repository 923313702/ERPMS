using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Util
{
    public class Tree
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string Url { get; set; }

        /// <summary>
        /// 班组标志（组织编码表用的）
        /// </summary>
        //public int? Group { get; set; }
        //public string Workshop { get; set; }
    }

    public class Node
    {
        [JsonProperty(PropertyName = "children")]
        public List<Node> Children = new List<Node>();

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Node Parent { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "parentId")]
        public string ParentId { get; set; }

        public string state { get; set; }
        [JsonProperty(PropertyName = "text")]
        public string Name { get; set; }

        public string Url { get; set; }
        public Dictionary<string, string> Attributes { get; set; }

        public static IEnumerable<Node> RawCollectionToTree(List<Tree> collection, bool flag)
        {
            var treeDictionary = new Dictionary<string, Node>();
            collection.ForEach(x => treeDictionary.Add(x.Id, new Node { Id = x.Id, ParentId = x.ParentId, Name = x.Name, Url = x.Url}));

            foreach (var item in treeDictionary.Values)
            {
                if (!string.IsNullOrEmpty(item.ParentId))
                {
                    Node proposedParent;


                    if (treeDictionary.TryGetValue(item.ParentId, out proposedParent))
                    {
                        item.Parent = proposedParent;
                        if (!string.IsNullOrEmpty(item.Url))
                        {
                            item.Attributes = new Dictionary<string, string>();
                            item.Attributes.Add("url", item.Url);
                        }
                        if (flag)
                        {
                            proposedParent.state = "closed";
                        }

                        proposedParent.Children.Add(item);
                    }
                }

            }
            return treeDictionary.Values.Where(x => x.Parent == null);
        }



    }




}