package com.microsoft.recognizers.text.choice;

import java.util.List;

import com.microsoft.recognizers.text.IModel;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.Recognizer;

public class ChoiceRecognizer extends Recognizer<ChoiceOptions>{

	public ChoiceRecognizer(String targetCulture, ChoiceOptions options, boolean lazyInitialization){
		super(targetCulture,options,lazyInitialization);
	}

	public ChoiceRecognizer(String targetCulture, int options, boolean lazyInitialization){
		this(targetCulture, ChoiceOptions.values()[options], lazyInitialization);
	}

	public ChoiceRecognizer(int options, boolean lazyInitialization){
		this(null, ChoiceOptions.values()[options], lazyInitialization);
	}

	public ChoiceRecognizer(ChoiceOptions options, boolean lazyInitialization){
		this(null, options, lazyInitialization);
	}

	public ChoiceRecognizer(boolean lazyInitialization){
		this(null, ChoiceOptions.None, lazyInitialization);
	}

	public ChoiceRecognizer(int options){
		this(null, ChoiceOptions.values()[options], true);
	}

	public ChoiceRecognizer(ChoiceOptions options){
		this(null, options, true);
	}

	public ChoiceRecognizer(){
		this(null, ChoiceOptions.None, true);
	}

	public IModel GetBooleanModel(String culture, boolean fallbackToDefaultCulture){
		throw new UnsupportedOperationException();
		//return GetModel<BooleanModel>(culture, fallbackToDefaultCulture);
	}

	public static List<ModelResult> RecognizeBoolean(String query, String culture, ChoiceOptions options, boolean fallbackToDefaultCulture){
		throw new UnsupportedOperationException();
	}
	public static List<ModelResult> RecognizeBoolean(String query, String culture, ChoiceOptions options){
		return RecognizeBoolean(query, culture, options, true);
	}
	public static List<ModelResult> RecognizeBoolean(String query, String culture){
		return RecognizeBoolean(query, culture, ChoiceOptions.None);
	}

	@Override
	protected void InitializeConfiguration(){
		throw new UnsupportedOperationException();
		/*RegisterModel<BooleanModel>(Culture.Chinese, (options) => new BooleanModel(new BooleanParser(), new BooleanExtractor(new ChineseBooleanExtractorConfiguration())));
		RegisterModel<BooleanModel>(Culture.Dutch, (options) => new BooleanModel(new BooleanParser(), new BooleanExtractor(new DutchBooleanExtractorConfiguration())));
		RegisterModel<BooleanModel>(Culture.English, (options) => new BooleanModel(new BooleanParser(), new BooleanExtractor(new EnglishBooleanExtractorConfiguration())));
		RegisterModel<BooleanModel>(Culture.French, (options) => new BooleanModel(new BooleanParser(), new BooleanExtractor(new FrenchBooleanExtractorConfiguration())));
		RegisterModel<BooleanModel>(Culture.German, (options) => new BooleanModel(new BooleanParser(), new BooleanExtractor(new GermanBooleanExtractorConfiguration())));
		RegisterModel<BooleanModel>(Culture.Japanese, (options) => new BooleanModel(new BooleanParser(), new BooleanExtractor(new JapaneseBooleanExtractorConfiguration())));
		RegisterModel<BooleanModel>(Culture.Portuguese, (options) => new BooleanModel(new BooleanParser(), new BooleanExtractor(new PortugueseBooleanExtractorConfiguration())));
		RegisterModel<BooleanModel>(Culture.Spanish, (options) => new BooleanModel(new BooleanParser(), new BooleanExtractor(new SpanishBooleanExtractorConfiguration())));*/
	}
}