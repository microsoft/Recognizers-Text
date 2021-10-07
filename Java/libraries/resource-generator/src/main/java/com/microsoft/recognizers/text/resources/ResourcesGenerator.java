// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.resources;

import com.fasterxml.jackson.databind.ObjectMapper;

import java.io.*;
import java.nio.file.FileSystems;
import java.nio.file.Path;
import java.util.stream.Collectors;

public class ResourcesGenerator {

    private static final String ResourcesPath = "../Patterns";

    private static final String LineBreak = "\n";

    public static void main(String[] args) throws Exception {
        if (args.length == 0) {
            throw new Exception("Please specify path to pattern/resource file.");
        }

        for (String resourceDefinitionFilePath : args) {

            ResourceDefinitions definition = Parse(resourceDefinitionFilePath);
            definition.configFiles.forEach(config -> {
                Path inputPath = FileSystems.getDefault().getPath(ResourcesPath, String.join(File.separator, config.input) + ".yaml");
                Path outputPath = FileSystems.getDefault().getPath(definition.outputPath, config.output + ".java");
                System.out.println(String.format("%s => %s", inputPath.toString(), outputPath.toString()));

                String header = String.join(LineBreak, config.header);
                String footer = String.join(LineBreak, config.footer);

                try {
                    CodeGenerator.Generate(inputPath, outputPath, header, footer);
                } catch (IOException e) {
                    e.printStackTrace();
                }
            });
        }
    }

    private static ResourceDefinitions Parse(String resourceDefinitionFile) throws IOException {
        Reader reader = new InputStreamReader(new FileInputStream(resourceDefinitionFile), "utf-8");
        BufferedReader br = new BufferedReader(reader);

        String json = br.lines().collect(Collectors.joining());

        return new ObjectMapper().readValue(json, ResourceDefinitions.class);
    }
}
