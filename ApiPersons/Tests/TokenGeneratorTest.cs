using ApiPersons.Utilities;
using NUnit.Framework;

namespace ApiPersons.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    public class TokenGeneratorTest
    {

        [Test]
        public void GenerateRandomToken_ReturnsCorrectLength()
        {
            // Arrange
            const int LENGTH_TOKEN = 16; // Ajusta la longitud del token según tus necesidades

            // Act
            string token = TokenGenerator.generateRandomToken();

            // Assert
            Assert.AreEqual(LENGTH_TOKEN * 2, token.Length); // El token debe tener longitud LENGTH_TOKEN * 2 debido a los bytes convertidos a hexadecimal
        }

        [Test]
        public void GenerateRandomToken_GeneratesUniqueTokens()
        {
            // Arrange
            const int NUM_TOKENS = 1000; // Número de tokens a generar
            HashSet<string> uniqueTokens = new HashSet<string>();

            // Act
            for (int i = 0; i < NUM_TOKENS; i++)
            {
                string token = TokenGenerator.generateRandomToken();
                uniqueTokens.Add(token);
            }

            // Assert
            Assert.AreEqual(NUM_TOKENS, uniqueTokens.Count); // Todos los tokens deben ser únicos
        }
    }
}
