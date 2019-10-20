﻿using System;
using System.Collections.Generic;
using System.Linq;
using Saxx.Storyblok.Attributes;

namespace Saxx.Storyblok
{
    public static class StoryblokMappings
    {
        private static IDictionary<string, Mapping> _mappingsCache; // make static, because it should be "valid" for as long as the app is running

        public static IDictionary<string, Mapping> Mappings
        {
            get
            {
                if (_mappingsCache == null)
                {
                    var components = from a in AppDomain.CurrentDomain.GetAssemblies()
                                     from t in a.GetTypes()
                                     let attributes = t.GetCustomAttributes(typeof(StoryblokComponentAttribute), true)
                                     where attributes != null && attributes.Length > 0
                                     select new { Type = t, Attribute = attributes.Cast<StoryblokComponentAttribute>().First() };

                    _mappingsCache = components.ToDictionary(x => x.Attribute.Name, x => new Mapping
                    {
                        Type = x.Type,
                        ComponentName = x.Attribute.Name,
                        View = x.Attribute.View
                    });
                }

                return _mappingsCache;
            }
        }

        public class Mapping
        {
            public string ComponentName { get; set; }
            public Type Type { get; set; }
            public string View { get; set; }
        }
    }
}