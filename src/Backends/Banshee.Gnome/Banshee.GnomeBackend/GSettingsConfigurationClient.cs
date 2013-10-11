//
// GSettingsConfigurationClient.cs
//
// Author:
//   Andres G. Aragoneses <knocte@gmail.com>
//
// Copyright 2012 Andres G. Aragoneses
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;

using Hyena;
using Banshee.Configuration;

namespace Banshee.GnomeBackend
{
    public class GSettingsConfigurationClient : IConfigurationClient
    {
        private static string base_schema = "org.gnome.banshee.";

        private List<string> schema_ids;

        private static string base_path;
        private static string BasePath {
            get {
                if (base_path == null) {
                    base_path = ApplicationContext.CommandLine["gsettings-base-path"];
                    if (!base_path.StartsWith ("/") || !base_path.EndsWith ("/")) {
                        Log.Debug ("Using default gsettings-base-path");
                        base_path = "/org/gnome/banshee/";
                    }
                }
                return base_path;
            }
        }

        public GSettingsConfigurationClient ()
        {
            schema_ids = new List<string> (GLib.Settings.ListRelocatableSchemas ());
            schema_ids.RemoveAll (s => !s.StartsWith (base_schema));
        }

        private string GetSchemaIdFromPath (string path)
        {
            string dotted_path = path.TrimStart ('/').Replace ('/', '.');
            while (!schema_ids.Contains (dotted_path)) {
                dotted_path = dotted_path.Remove (dotted_path.LastIndexOf ('.'));
            }
            Log.DebugFormat ("### GetSchemaIdFromPath {0} -> {1}", path, dotted_path);
            return dotted_path;
        }
     
        public bool TryGet<T> (string @namespace, string key, out T result)
        {
            result = default (T);
            Hyena.Log.DebugFormat ("### TryGet namespace={0} key={1}", @namespace, key);
            string path = String.Concat (BasePath, @namespace);
            string schema_id = GetSchemaIdFromPath (path);
            GLib.Settings settings;
            Hyena.Log.DebugFormat ("### GLib.Settings id={0} path={1} key={2}", schema_id, path, key);
            try {
                settings = new GLib.Settings (schema_id, path);

                // gsettings doesn't allow underscores in the keys
                result = (T)Get (typeof (T), settings, key.Replace ("_", "-"));
            } catch (Exception e) {
                Log.DebugException (e);
            }
            return true;
        }

        private object Get (Type type, GLib.Settings settings, string key)
        {
            if (type == typeof (bool)) {
                return settings.GetBoolean (key);
            }
            if (type == typeof (string)) {
                return settings.GetString (key);
            }
            if (type == typeof (int)) {
                return settings.GetInt (key);
            }
            if (type == typeof (double)) {
                return settings.GetDouble (key);
            }
            if (type == typeof (String[])) {
                return settings.GetStrv (key);
            }
            throw new NotImplementedException (String.Format ("Type {0} not supported in {1}",
                                                              type.FullName, 
                                                              this.GetType ().Name));
        }

        public void Set<T> (string @namespace, string key, T value)
        {
            throw new NotImplementedException ("SET not yet! for " + @namespace + "=>" + key);
        }

        public void Set<T> (string @namespace, string path, string key, T value)
        {
            throw new NotImplementedException ("SET not yet! for " + @namespace + "=>" + key);
        }
    }
}

