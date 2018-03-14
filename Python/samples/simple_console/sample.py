import json
from typing import List
from recognizers_number import Culture, ModelResult, NumberRecognizer
DEFAULT_CULTURE = Culture.English

def main():
    show_intro()
    run_recognition()

# Read from Console and recognize
def run_recognition():
    user_input: str = ''
    while user_input.lower() != 'exit':
        user_input = str(input('Enter the text to recognize: ')).strip()
        if user_input not in ['', 'exit']:
            # Retrieve all the ModelResult recognized from the user input
            results = parse_all(user_input, DEFAULT_CULTURE)
            # Flatten results
            results = [item for sublist in results for item in sublist]

            # Write results on console
            print()
            print(f'I found the following entities ({len(results)}):'
                  if results else 'I found no entities.')
            print()

            for result in results:
                print(json.dumps(result, default=lambda o: o.__dict__, indent='\t'))
                print()

def parse_all(user_input: str, culture: str) -> List[ModelResult]:
    return [
        # Number recognizer - This function will find any number from the input
        # E.g "I have two apples" will return "2".
        NumberRecognizer.recognize_number(user_input, culture),
        # Ordinal number recognizer - This function will find any ordinal number
        # E.g "eleventh" will return "11".
        NumberRecognizer.recognize_ordinal(user_input, culture),
        # Percentage recognizer - This function will find any number presented as percentage
        # E.g "one hundred percents" will return "100%"
        NumberRecognizer.recognize_percentage(user_input, culture),
    ]

# Show Introduction
def show_intro():
    print('''Welcome to the Recognizer\'s Sample console application!
To try the recognizers enter a phrase and let us show you the different\
 outputs for each recognizer or just type \'exit\' to leave the application.

Here are some examples you could try:

\" I want twenty meters of cable for tomorrow\"
\" I\'ll be available tomorrow from 11am to 2pm to receive up to 5kg of sugar\"
\" I\'ll be out between 4 and 22 this month\"
\" I was the fifth person to finish the 10 km race\"
\" The temperature this night will be of 40 deg celsius\"
\" The american stock exchange said a seat was sold for down $ 5,000 from the\
 previous sale last friday\"
\" It happened when the baby was only ten months old\"
\" No, I don\'t think that we can make 100k USD today\"
''')

if __name__ == '__main__':
    main()
