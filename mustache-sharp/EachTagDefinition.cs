﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Mustache
{
    /// <summary>
    /// Defines a tag that can iterate over a collection of items and render
    /// the content using each item as the context.
    /// </summary>
    internal sealed class EachTagDefinition : ContentTagDefinition
    {
        private const string collectionParameter = "collection";
        private static readonly TagParameter collection = new TagParameter(collectionParameter) { IsRequired = true };

        /// <summary>
        /// Initializes a new instance of an EachTagDefinition.
        /// </summary>
        public EachTagDefinition()
            : base("each", true)
        {
        }

        /// <summary>
        /// Gets whether the tag only exists within the scope of its parent.
        /// </summary>
        protected override bool GetIsContextSensitive()
        {
            return false;
        }

        /// <summary>
        /// Gets the parameters that can be passed to the tag.
        /// </summary>
        /// <returns>The parameters.</returns>
        protected override IEnumerable<TagParameter> GetParameters()
        {
            return new TagParameter[] { collection };
        }

        /// <summary>
        /// Gets the context to use when building the inner text of the tag.
        /// </summary>
        /// <param name="writer">The text writer passed</param>
        /// <param name="scope">The current scope.</param>
        /// <param name="arguments">The arguments passed to the tag.</param>
        /// <returns>The scope to use when building the inner text of the tag.</returns>
        public override IEnumerable<NestedContext> GetChildContext(TextWriter writer, KeyScope scope, Dictionary<string, object> arguments)
        {
            object value = arguments[collectionParameter];
            IEnumerable enumerable = value as IEnumerable;
            if (enumerable == null)
            {
                yield break;
            }
            foreach (object item in enumerable)
            {
                yield return new NestedContext() { KeyScope = scope.CreateChildScope(item), Writer = writer };
            }
        }

        /// <summary>
        /// Gets the tags that are in scope under this tag.
        /// </summary>
        /// <returns>The name of the tags that are in scope.</returns>
        protected override IEnumerable<string> GetChildTags()
        {
            return new string[] { };
        }

        /// <summary>
        /// Gets the parameters that are used to create a new child context.
        /// </summary>
        /// <returns>The parameters that are used to create a new child context.</returns>
        public override IEnumerable<TagParameter> GetChildContextParameters()
        {
            return new TagParameter[] { collection };
        }
    }
}
