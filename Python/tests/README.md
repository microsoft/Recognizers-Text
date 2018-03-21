# Python Tests

In order to verify the correct behavior of the Python Recognizers, the same [Specs suite](../../Specs) is shared among platforms.

The Python test runner is implemented as a [parameterized](https://docs.pytest.org/en/latest/reference.html#pytest-mark-parametrize) [pytest](https://docs.pytest.org/) fixture for each recognizer type which is excuted for each spec.

## Running the Specs

Running spec tests are included as part of of the automatied build: `Build.cmd`

Specs can also be run manually executing `pytest` from the command line.
```
cd .\python
pytest
```

You can install test requirements from the command line executing:
```
pip install -r .\python\tests\requirements.txt
```
