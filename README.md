# CryptSharp
Classical ciphers for C#

Among the implemented Ciphers are FourSquare, Hill, Vigenere and many others

There are 3 (incomplete) projects within this solution.
1. CryptSharp - a dll that contains some classical ciphers
2. CryptSharp.Test- a project to test the ciphers
3. Cryptalize - a desktop app to analyze classical ciphers and hopefully be a game to generate some for learning

Utility
-------
In CryptSharp there is a class called Utility.  Within this class are several functions to help with generic tasks associated with ciphers and cryptanalysis.

Usage
-----
General usage is as follows:
```
Cipher variableName = new Cipher(alphabetCharArray);
variableName.Key = "CRYPTOTRICK";

string cipherText = variableName.Encrypt(clearText);
string clearText = variableName.Decrypt(cipherText);
```

Not all ciphers have a "Key", some have arrays and other such things.
