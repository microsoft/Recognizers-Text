=============================================
Microsoft.Recognizers.Text.Number for Python
=============================================

Installation
------------
Install NumberRecognizer by launching the following command:

``pip install recognizers-text-number``

API Documentation
-----------------
Once the proper package is installed, you'll need to reference the package:

.. code-block:: python

  from recognizers_number import Culture, ModelResult, NumberRecognizer

Recognizer's Models
~~~~~~~~~~~~~~~~~~~
This is the preferred way if you need to parse multiple inputs based on the same context (e.g.: language and options):

.. code-block:: python 

   recognizer = NumberRecognizer(Culture.English) 
   model = recognizer.get_number_model() 
   result = model.parse('Twelve')

Or, for less verbosity, you use the helper methods:

``result = NumberRecognizer.recognize_number("Twelve", Culture.English);``

Internally, both methods will cache the instance models to avoid extra
costs.

Microsoft.Recognizers.Text.Number
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

-  **Numbers**

   This recognizer will find any number from the input. E.g. *"I have
   two apples"* will output ``Received "two", resolution is: {'value': '2'}``.

   .. code-block:: python 

    result = NumberRecognizer.recognize_number('I have two apples', Culture.English)
    print(f'Received "{result[0].text}", resolution is: {result[0].resolution}')

   Or you can obtain a model instance using:

   ``NumberRecognizer(Culture.English).get_number_model()``

-  **Ordinal Numbers**

   This recognizer will find any ordinal number. E.g. *"eleventh"* will
   output ``Received "eleventh", resolution is: {'value': '11'}``.

   .. code-block:: python 

    result = NumberRecognizer.recognize_ordinal('eleventh', Culture.English)
    print(f'Received "{result[0].text}", resolution is: {result[0].resolution}')

   Or you can obtain a model instance using:

   ``NumberRecognizer(Culture.English).get_ordinal_model()``

-  **Percentages**

   This recognizer will find any number presented as percentage. E.g.
   *"one hundred percents"* will output ``Received "one hundred percents", resolution is: {'value': '100%'}``.

   .. code-block:: python 

    result = NumberRecognizer.recognize_percentage('one hundred percents', Culture.English)
    print(f'Received "{result[0].text}", resolution is: {result[0].resolution}')

   Or you can obtain a model instance using:

   ``NumberRecognizer(Culture.English).get_percentage_model()``


This module (``recognizers-text-number``) is a sub-module of
``recognizers-text-suite``.

Please check the `main README`_ for more details.

.. _main README: https://github.com/Microsoft/Recognizers-Text/tree/master/Python