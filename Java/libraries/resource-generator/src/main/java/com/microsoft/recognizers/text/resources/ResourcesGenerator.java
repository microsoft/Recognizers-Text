package com.microsoft.recognizers.text.resources;

import com.fasterxml.jackson.databind.ObjectMapper;

import java.io.File;
import java.io.IOException;
import java.nio.file.FileSystems;
import java.nio.file.Files;
import java.nio.file.Path;

public class ResourcesGenerator {

    private static final String ResourcesPath = "../Patterns";

    public static void main(String[] args) throws Exception {
        if (args.length == 0) {
            throw new Exception("Please specify path to pattern/resource file.");
        }

        for(String resourceDefinitionFilePath : args) {

            ResourceDefinitions definition = Parse(resourceDefinitionFilePath);
            definition.configFiles.forEach(config -> {
                Path inputPath = FileSystems.getDefault().getPath(ResourcesPath, String.join(File.separator, config.input) + ".yaml");
                Path outputPath = FileSystems.getDefault().getPath(definition.outputPath, config.output + ".java");
                System.out.println(String.format("%s => %s", inputPath.toString(), outputPath.toString()));

                String header = String.join(System.lineSeparator(), config.header);
                String footer = String.join(System.lineSeparator(), config.footer);

                try {
                    CodeGenerator.Generate(inputPath, outputPath, header, footer);
                } catch (IOException e) {
                    e.printStackTrace();
                }
            });
        }
    }

    private static ResourceDefinitions Parse(String resourceDefinitionFile) throws IOException {
        Path path = FileSystems.getDefault().getPath(resourceDefinitionFile);
        String json = new String(Files.readAllBytes(path));

        return new ObjectMapper().readValue(json, ResourceDefinitions.class);
    }
}
