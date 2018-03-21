=====================================================
Microsoft.Recognizers.Text.NumberWithUnit for Python
=====================================================

Installation
------------
Install NumberWithUnitRecognizer by launching the following command:

``pip install recognizers-text-number-with-unit``

API Documentation
-----------------
Once the proper package is installed, you'll need to reference the package:

.. code-block:: python

  from recognizers_text import Culture, ModelResult
  from recognizers_number_with_unit import NumberWithUnitRecognizer

Recognizer's Models
~~~~~~~~~~~~~~~~~~~
This is the preferred way if you need to parse multiple inputs based on the same context (e.g.: language and options):

.. code-block:: python 

   recognizer = NumberWithUnitRecognizer(Culture.English) 
   model = recognizer.get_currency_model() 
   result = model.parse('$ 75.3 million')

Or, for less verbosity, you use the helper methods:

``result = NumberWithUnitRecognizer.recognize_currency("Twelve", Culture.English);``

Internally, both methods will cache the instance models to avoid extra
costs.

Microsoft.Recognizers.Text.NumberWithUnit
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

-  **Ages**

   This recognizer will find any age number presented. E.g. *"After ninety five years of age, perspectives change"*
   will output ``Received "ninety five years of age", resolution is: ValueUnit(value='95', unit='Year')``.

   .. code-block:: python 

    result = NumberWithUnitRecognizer.recognize_age('After ninety five years of age, perspectives change', Culture.English)
    print(f'Received "{result[0].text}", resolution is: {result[0].resolution}')

   Or you can obtain a model instance using:

   ``NumberWithUnitRecognizer(Culture.English).get_age_model()``


-  **Currencies**

   This recognizer will find any currency presented. E.g. *"Interest expense in the 1988 third quarter was $ 75.3 million"*
   will output ``Received "$ 75.3 million", resolution is: ValueUnit(value='75300000', unit='Dollar')``.

   .. code-block:: python 

    result = NumberWithUnitRecognizer.recognize_currency('Interest expense in the 1988 third quarter was $ 75.3 million', Culture.English)
    print(f'Received "{result[0].text}", resolution is: {result[0].resolution}')

   Or you can obtain a model instance using:

   ``NumberWithUnitRecognizer(Culture.English).get_currency_model()``


-  **Dimensions**

   This recognizer will find any dimension presented. E.g. *"The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours."*
   will output ``Received "six-mile", resolution is: ValueUnit(value='6', unit='Mile')``.

   .. code-block:: python 

    result = NumberWithUnitRecognizer.recognize_dimension('The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours.', Culture.English)
    print(f'Received "{result[0].text}", resolution is: {result[0].resolution}')

   Or you can obtain a model instance using:

   ``NumberWithUnitRecognizer(Culture.English).get_dimension_model()``


-  **Temperatures**

   This recognizer will find any temperature presented. E.g. *"Set the temperature to 30 degrees celsius"*
   will output ``Received "30 degrees celsius", resolution is: ValueUnit(value='30', unit='C')``.

   .. code-block:: python 

    result = NumberWithUnitRecognizer.recognize_temperature('Set the temperature to 30 degrees celsius', Culture.English)
    print(f'Received "{result[0].text}", resolution is: {result[0].resolution}')

   Or you can obtain a model instance using:

   ``NumberWithUnitRecognizer(Culture.English).get_temperature_model()``

This module (``recognizers-text-number-with-unit``) is a sub-module of
``recognizers-text-suite``.

Please check the `main README`_ for more details.

.. _main README: https://github.com/Microsoft/Recognizers-Text/tree/master/Python