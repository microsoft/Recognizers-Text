// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text
{
    public abstract class Recognizer<TRecognizerOptions>
           where TRecognizerOptions : struct
    {
        private static readonly IDictionary<Type, int> TimeoutDictionary = new Dictionary<Type, int>();

        private static readonly object _locker = new object();

        private readonly ModelFactory<TRecognizerOptions> factory;

        protected Recognizer(string targetCulture, TRecognizerOptions options, bool lazyInitialization, int timeout = 0)
        {
            this.Options = options;
            this.TargetCulture = targetCulture;
            this.TimeoutInSeconds = timeout;
            this.factory = new ModelFactory<TRecognizerOptions>();

            AddRegexTimeoutValuesForType();

            InitializeConfiguration();

            if (!lazyInitialization)
            {
                this.InitializeModels(targetCulture, options);
            }
        }

        public string TargetCulture { get; private set; }

        public TRecognizerOptions Options { get; private set; }

        protected int TimeoutInSeconds { get; }

        public static TRecognizerOptions GetOptions(int value) => EnumUtils.Convert<TRecognizerOptions>(value);

        public static TimeSpan GetTimeout(Type type)
        {
            return TimeoutDictionary.TryGetValue(type, out var timeInSeconds) && timeInSeconds > 0 ?
                TimeSpan.FromSeconds(timeInSeconds) : TimeSpan.FromSeconds(Constants.MaxRegexTimeoutInSeconds);
        }

        // For each Recognizer type (i.e., NumberRecognizer, DateTimeRecognizer, SequenceRecognizer and so on)
        // Find all the types that should use the same timeout value set by that recognizer.
        // Refer to the concrete implementation of each recognizer for the list of the types. These are
        // the types that have a Regex object created in them and need a Timeout parameter.
        protected abstract List<Type> GetRelatedTypes();

        protected T GetModel<T>(string culture, bool fallbackToDefaultCulture)
                  where T : IModel
        {
            return this.factory.GetModel<T>(culture ?? TargetCulture, fallbackToDefaultCulture, Options);
        }

        protected void RegisterModel<T>(string culture, Func<TRecognizerOptions, IModel> modelCreator)
        {
            this.factory.Add((culture, typeof(T)), modelCreator);
        }

        protected abstract void InitializeConfiguration();

        private void InitializeModels(string targetCulture, TRecognizerOptions options)
        {
            this.factory.InitializeModels(targetCulture, options);
        }

        private void AddRegexTimeoutValuesForType()
        {
            // Foreach Recognizer type find the subtypes who are supposed to use the same
            // Regex timeout value. Children of Recognzier get to have their own timeout value.
            lock (_locker)
            {
                if (!TimeoutDictionary.ContainsKey(this.GetType()))
                {
                    TimeoutDictionary.Add(this.GetType(), TimeoutInSeconds);
                    var relatedTypes = GetRelatedTypes();
                    relatedTypes.ForEach(t => TimeoutDictionary.Add(t, TimeoutInSeconds));
                }
            }
        }
    }
}