// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.IModel;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.Recognizer;
import com.microsoft.recognizers.text.sequence.config.BaseSequenceConfiguration;
import com.microsoft.recognizers.text.sequence.english.extractors.EmailExtractor;
import com.microsoft.recognizers.text.sequence.english.extractors.EnglishIpExtractorConfiguration;
import com.microsoft.recognizers.text.sequence.english.extractors.EnglishPhoneNumberExtractorConfiguration;
import com.microsoft.recognizers.text.sequence.english.extractors.EnglishURLExtractorConfiguration;
import com.microsoft.recognizers.text.sequence.english.extractors.GUIDExtractor;
import com.microsoft.recognizers.text.sequence.english.extractors.HashTagExtractor;
import com.microsoft.recognizers.text.sequence.english.extractors.MentionExtractor;
import com.microsoft.recognizers.text.sequence.english.parsers.EmailParser;
import com.microsoft.recognizers.text.sequence.english.parsers.GUIDParser;
import com.microsoft.recognizers.text.sequence.english.parsers.HashTagParser;
import com.microsoft.recognizers.text.sequence.english.parsers.IpParser;
import com.microsoft.recognizers.text.sequence.english.parsers.MentionParser;
import com.microsoft.recognizers.text.sequence.english.parsers.PhoneNumberParser;
import com.microsoft.recognizers.text.sequence.english.parsers.URLParser;
import com.microsoft.recognizers.text.sequence.extractors.BaseIpExtractor;
import com.microsoft.recognizers.text.sequence.extractors.BasePhoneNumberExtractor;
import com.microsoft.recognizers.text.sequence.extractors.BasePhoneNumberExtractorConfiguration;
import com.microsoft.recognizers.text.sequence.extractors.BaseURLExtractor;
import com.microsoft.recognizers.text.sequence.models.EmailModel;
import com.microsoft.recognizers.text.sequence.models.GUIDModel;
import com.microsoft.recognizers.text.sequence.models.HashTagModel;
import com.microsoft.recognizers.text.sequence.models.IpAddressModel;
import com.microsoft.recognizers.text.sequence.models.MentionModel;
import com.microsoft.recognizers.text.sequence.models.PhoneNumberModel;
import com.microsoft.recognizers.text.sequence.models.URLModel;

import java.util.List;
import java.util.Locale;
import java.util.function.Function;

public class SequenceRecognizer extends Recognizer<SequenceOptions> {
    public SequenceRecognizer() {
        this(null, SequenceOptions.None, true);
    }

    public SequenceRecognizer(String culture) {
        this(culture, SequenceOptions.None, false);
    }

    public SequenceRecognizer(String targetCulture, SequenceOptions options, boolean lazyInitialization) {
        super(targetCulture, options, lazyInitialization);
    }

    public SequenceRecognizer(String targetCulture, int options, boolean lazyInitialization) {
        this(targetCulture, SequenceOptions.values()[options], lazyInitialization);
    }

    public SequenceRecognizer(int options, boolean lazyInitialization) {
        this(null, SequenceOptions.values()[options], lazyInitialization);
    }

    public SequenceRecognizer(SequenceOptions options, boolean lazyInitialization) {
        this(null, options, lazyInitialization);
    }

    public SequenceRecognizer(boolean lazyInitialization) {
        this(null, SequenceOptions.None, lazyInitialization);
    }

    public SequenceRecognizer(int options) {
        this(null, SequenceOptions.values()[options], true);
    }

    public SequenceRecognizer(SequenceOptions options) {
        this(null, options, true);
    }

    public static List<ModelResult> recognizePhoneNumber(String query, String culture) {
        return recognizePhoneNumber(query, culture, SequenceOptions.None, true);
    }

    public static List<ModelResult> recognizePhoneNumber(String query, String culture, SequenceOptions options,
            Boolean fallbackToDefaultCulture) {
        options = options != null ? options : SequenceOptions.None;
        return SequenceRecognizer.recognizeByModel(recognizer -> ((SequenceRecognizer)recognizer).getPhoneNumberModel(culture, fallbackToDefaultCulture),
                query, options);
    }

    public static List<ModelResult> recognizeIpAddress(String query, String culture) {
        return recognizeIpAddress(query, culture, SequenceOptions.None, true);
    }

    public static List<ModelResult> recognizeIpAddress(String query, String culture, SequenceOptions options,
            Boolean fallbackToDefaultCulture) {
        options = options != null ? options : SequenceOptions.None;
        return SequenceRecognizer.recognizeByModel(recognizer -> ((SequenceRecognizer)recognizer).getIpAddressModel(culture, fallbackToDefaultCulture),
                query, options);
    }

    public static List<ModelResult> recognizeMention(String query, String culture) {
        return recognizeMention(query, culture, SequenceOptions.None, true);
    }

    public static List<ModelResult> recognizeMention(String query, String culture, SequenceOptions options,
            Boolean fallbackToDefaultCulture) {
        options = options != null ? options : SequenceOptions.None;
        return SequenceRecognizer.recognizeByModel(recognizer -> ((SequenceRecognizer)recognizer).getMentionModel(culture, fallbackToDefaultCulture),
                query, options);
    }

    public static List<ModelResult> recognizeHashtag(String query, String culture) {
        return recognizeHashtag(query, culture, SequenceOptions.None, true);
    }

    public static List<ModelResult> recognizeHashtag(String query, String culture, SequenceOptions options,
            Boolean fallbackToDefaultCulture) {
        options = options != null ? options : SequenceOptions.None;
        return SequenceRecognizer.recognizeByModel(recognizer -> ((SequenceRecognizer)recognizer).getHashtagModel(culture, fallbackToDefaultCulture),
                query, options);
    }

    public static List<ModelResult> recognizeEmail(String query, String culture) {
        return recognizeEmail(query, culture, SequenceOptions.None, true);
    }

    public static List<ModelResult> recognizeEmail(String query, String culture, SequenceOptions options,
            Boolean fallbackToDefaultCulture) {
        options = options != null ? options : SequenceOptions.None;
        return SequenceRecognizer.recognizeByModel(recognizer -> ((SequenceRecognizer)recognizer).getEmailModel(culture, fallbackToDefaultCulture), query,
                options);
    }

    public static List<ModelResult> recognizeURL(String query, String culture) {
        return recognizeURL(query, culture, SequenceOptions.None, true);
    }

    public static List<ModelResult> recognizeURL(String query, String culture, SequenceOptions options,
            Boolean fallbackToDefaultCulture) {
        options = options != null ? options : SequenceOptions.None;
        return SequenceRecognizer.recognizeByModel(recognizer -> ((SequenceRecognizer)recognizer).getURLModel(culture, fallbackToDefaultCulture), query,
                options);
    }

    public static List<ModelResult> recognizeGUID(String query, String culture) {
        return recognizeGUID(query, culture, SequenceOptions.None, true);
    }

    public static List<ModelResult> recognizeGUID(String query, String culture, SequenceOptions options,
            Boolean fallbackToDefaultCulture) {
        options = options != null ? options : SequenceOptions.None;
        return SequenceRecognizer.recognizeByModel(recognizer -> ((SequenceRecognizer)recognizer).getGUIDModel(culture, fallbackToDefaultCulture), query,
                options);
    }

    //region Helper methods for less verbosity
    public IModel getPhoneNumberModel() {
        return getPhoneNumberModel(null, true);
    }

    public IModel getPhoneNumberModel(String culture, Boolean fallbackToDefaultCulture) {
        fallbackToDefaultCulture = fallbackToDefaultCulture != null ? fallbackToDefaultCulture : true;
        if (culture != null && (culture.toLowerCase(Locale.ROOT).startsWith("zh-") ||
            culture.toLowerCase(Locale.ROOT).startsWith("ja-"))) {
            return this.getModel(PhoneNumberModel.class, Culture.Chinese, fallbackToDefaultCulture);
        }

        return this.getModel(PhoneNumberModel.class, culture, fallbackToDefaultCulture);
    }

    public IModel getIpAddressModel(String culture, Boolean fallbackToDefaultCulture) {
        fallbackToDefaultCulture = fallbackToDefaultCulture != null ? fallbackToDefaultCulture : true;
        if (culture != null && (culture.toLowerCase(Locale.ROOT).startsWith("zh-") || culture.toLowerCase(Locale.ROOT).startsWith("ja-"))) {
            return this.getModel(IpAddressModel.class, Culture.Chinese, fallbackToDefaultCulture);
        }

        return this.getModel(IpAddressModel.class, Culture.English, fallbackToDefaultCulture);
    }

    public IModel getMentionModel(String culture, Boolean fallbackToDefaultCulture) {
        fallbackToDefaultCulture = fallbackToDefaultCulture != null ? fallbackToDefaultCulture : true;
        return this.getModel(MentionModel.class, Culture.English, fallbackToDefaultCulture);
    }

    public IModel getHashtagModel(String culture, Boolean fallbackToDefaultCulture) {
        fallbackToDefaultCulture = fallbackToDefaultCulture != null ? fallbackToDefaultCulture : true;
        return this.getModel(HashTagModel.class, Culture.English, fallbackToDefaultCulture);
    }

    public IModel getEmailModel(String culture, Boolean fallbackToDefaultCulture) {
        fallbackToDefaultCulture = fallbackToDefaultCulture != null ? fallbackToDefaultCulture : true;
        return this.getModel(EmailModel.class, Culture.English, fallbackToDefaultCulture);
    }

    public IModel getURLModel(String culture, Boolean fallbackToDefaultCulture) {
        fallbackToDefaultCulture = fallbackToDefaultCulture != null ? fallbackToDefaultCulture : true;
        if (culture != null && (culture.toLowerCase(Locale.ROOT).startsWith("zh-") ||
            culture.toLowerCase(Locale.ROOT).startsWith("ja-"))) {
            return this.getModel(URLModel.class, Culture.Chinese, fallbackToDefaultCulture);
        }

        return this.getModel(URLModel.class, Culture.English, fallbackToDefaultCulture);
    }

    public IModel getGUIDModel(String culture, Boolean fallbackToDefaultCulture) {
        fallbackToDefaultCulture = fallbackToDefaultCulture != null ? fallbackToDefaultCulture : true;
        return this.getModel(GUIDModel.class, Culture.English, fallbackToDefaultCulture);
    }

    @Override
    protected void initializeConfiguration() {
        this.registerModel(PhoneNumberModel.class, Culture.English, (options) -> new PhoneNumberModel(new PhoneNumberParser(),
                        new BasePhoneNumberExtractor(new EnglishPhoneNumberExtractorConfiguration(options))));

        this.registerModel(PhoneNumberModel.class, Culture.Spanish, (options) -> new PhoneNumberModel(
            new PhoneNumberParser(), new BasePhoneNumberExtractor(new BasePhoneNumberExtractorConfiguration(options))));

        this.registerModel(IpAddressModel.class, Culture.English, (options) -> new IpAddressModel(new IpParser(),
                new BaseIpExtractor(new EnglishIpExtractorConfiguration(options))));

        this.registerModel(MentionModel.class, Culture.English, (options) -> new MentionModel(new MentionParser(), new MentionExtractor()));

        this.registerModel(HashTagModel.class, Culture.English, (options) -> new HashTagModel(new HashTagParser(), new HashTagExtractor()));

        this.registerModel(EmailModel.class, Culture.English, (options) -> new EmailModel(new EmailParser(new BaseSequenceConfiguration(options)),
                        new EmailExtractor(new BaseSequenceConfiguration(options))));

        this.registerModel(URLModel.class, Culture.English, (options) -> new URLModel(new URLParser(),
                new BaseURLExtractor(new EnglishURLExtractorConfiguration(options))));

        this.registerModel(GUIDModel.class, Culture.English, (options) -> new GUIDModel(new GUIDParser(), new GUIDExtractor()));
    }

    private static List<ModelResult> recognizeByModel(Function getModelFunc, String query, SequenceOptions options) {
        SequenceRecognizer recognizer = new SequenceRecognizer(options, false);
        IModel model = (IModel)getModelFunc.apply(recognizer);
        return model.parse(query);
    }
}
