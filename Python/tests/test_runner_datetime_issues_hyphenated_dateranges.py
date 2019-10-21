import recognizers_suite as Recognizers
from recognizers_suite import Culture, ModelResult
culture = Culture.English

# this should be recognized as a 'DateRange'. Instead, the type resolved is 'date'
#input = "10/1-11/5"

# this is incorrect too
input = "Nov-Dec 2015"
#input = "Nov 2015 - Dec 2015"
#input = "My vacation is from 10/1/2018 - 10/7/2018"
results = Recognizers.recognize_datetime(input, culture)
for result in results:
    print("text: {}".format(result.text))
    print("type_name: {}".format(result.type_name))
    print("resolution: {}".format(result.resolution))
