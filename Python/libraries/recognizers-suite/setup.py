#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

import os
from setuptools import setup, find_packages


def read(fname):
    return open(os.path.join(os.path.dirname(__file__), fname)).read()


NAME = 'recognizers-text-suite-genesys'
VERSION = '1.0.57'
REQUIRES = [
    'recognizers-text-genesys==1.0.57',
    'recognizers-text-number-genesys==1.0.57',
    'recognizers-text-number-with-unit-genesys==1.0.57',
    'recognizers-text-date-time-genesys==1.0.57',
    'recognizers-text-sequence-genesys==1.0.57',
    'recognizers-text-choice-genesys==1.0.57',
    'datatypes_timex_expression_genesys==1.0.57'
]

setup(
    name=NAME,
    version=VERSION,
    url='https://github.com/Microsoft/Recognizers-Text',
    author='Microsoft',
    description='recognizers-text-suite README',
    keywords=['nlp', 'nlp-entity-extraction',
              'entity-extraction', 'parser-library'],
    long_description=read('README.rst'),
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
