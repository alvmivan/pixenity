using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    [RequireComponent(typeof(Button))]
    public class DataButton : MonoBehaviour
    {   
        [SerializeField] List<StringPair> dict = new List<StringPair>();

        
        private Lazy<Button> buttonLazy;

        public DataButton()
        {
            buttonLazy = new Lazy<Button>(GetComponent<Button>);
        }

        public Button Button => buttonLazy.Value;


        public void Clear()
        {
            dict.Clear();
        }

       
       
        public int Count => dict.Count;

        public bool ContainsKey(string key)
        {
            return dict.Any(MatchPredicate(key));
        }

        public void Remove(string key)
        {
            dict.RemoveAll(a=>MatchPredicate(key)(a));
        }

        private static Func<StringPair,bool> MatchPredicate(string key)
        {
            return p=> string.Equals(p.key, key, StringComparison.Ordinal);
        }

        public bool TryGetValue(string key, out string value)
        {
            if (ContainsKey(key))
            {
                value = this[key];
                return true;
            }

            value = "";
            return false;
        }

        public string this[string key]
        {
            get
            {
                if(ContainsKey(key))
                    return dict.FirstOrDefault(MatchPredicate(key)).value;
                return string.Empty;
            }
            set
            {
                if (ContainsKey(key))
                {
                    Remove(key);
                }
                dict.Add(new StringPair
                {
                    key = key,
                    value = value
                });
            }
        }
    }

    [Serializable]struct StringPair
    {
        public string key;
        public string value;
    }
}