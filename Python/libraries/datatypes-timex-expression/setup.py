# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.

from setuptools import setup, find_packages

NAME = "datatypes-timex-expression"
VERSION = "1.0.0.a0"
REQUIRES = [regex']

setup(
    name=NAME,
    version=VERSION,
    url='https://github.com/Microsoft/Recognizers-Text',
    author='Microsoft',
    description='datatypes-timex-expression README',
    keywords=[ 'nlp', 'nlp-entity-extraction', 'entity-extraction', 'parser-library', 'timex' ],
    long_description='datatypes-timex-expression long README.',
    license='MIT',
    packages=find_packages(),
    install_requires=REQUIRES,
    classifiers=[
        'Programming Language :: Python :: 3.6',
        'Intended Audience :: Developers',
        'License :: OSI Approved :: MIT License',
        'Operating System :: OS Independent',
        'Development Status :: 3 - Alpha',
        'Topic :: Scientific/Engineering :: Artificial Intelligence',
    ]
)
