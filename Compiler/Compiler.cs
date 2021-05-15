using System;
using System.Collections.Generic;

namespace Compiler
{
    public class Compiler
    {
        private const string unexpectedTokenErrorMsg = "Encountered with an unexpected token. The expected token after the pointed token was";
        private const string unIdentifiedTokenErrorMsg = "Encountered with an unidentified token. The pointed token is not met by compiler.";
        private const string succesfullCompileMsg = "Source code compiled successfuly.";
        private const string smallCharacters = "abcdefghijklmnopqrstuvwxyz";
        private const string numbers = "0123456789";
    
        private string errorMsg;
        private List<string> _sourceCode;
        private int _lineIndex;
        private int _charIndex;

        public void Compile(List<string> sourceCode)
        {
            Reset();
            _sourceCode = sourceCode;
            if(P()) Console.WriteLine(succesfullCompileMsg);
            else
            {
                var lineWithError = _sourceCode[_lineIndex];
                string pointer = "";
                for (int i = 0; i < lineWithError.Length; i++)
                {
                    if (i == _charIndex) pointer += "^";
                    else pointer += " ";
                }
                Console.WriteLine(lineWithError);
                Console.WriteLine(pointer);
                Console.WriteLine(errorMsg);
            }
        }
        
        private bool P()
        {
            while (true)
            {
                if (!TryLookAHead(out char nextToken) || (nextToken != '[' && nextToken != '{' && !IsSmallLetter(nextToken) && nextToken != '<' && nextToken != '>' && nextToken != '.'))
                {
                    errorMsg = unexpectedTokenErrorMsg + " '[' or '{' or a small letter or '<' or '>' or '.'.";
                    return false;
                }

                if (nextToken == '.')
                {
                    return true;
                }

                if (!C())
                {
                    return false;
                }
            }
        }

        private bool C()
        {
            if (!TryLookAHead(out char nextToken))
            {
                errorMsg = unexpectedTokenErrorMsg + " '[' or '{' or a small letter or '<' or '>'.";
                return false;
            }
            
            //I or W or A or Ç or G
            if (nextToken == '[')
            {
                if (!I())
                {
                    return false;
                }
            }
            else if (nextToken == '{')
            {
                if (!W())
                {
                    return false;
                }
            }
            else if (IsSmallLetter(nextToken))
            {
                if (!A())
                {
                    return false;
                }
            }
            else if (nextToken == '<')
            {
                if (!Ç())
                {
                    return false;
                }
            }
            else if (nextToken == '>')
            {
                if (!G())
                {
                    return false;
                }
            }
            else
            {
                errorMsg = unexpectedTokenErrorMsg + " '[' or '{' or a small letter or '<' or '>'.";
                return false;
            }

            return true;
        }

        private bool I()
        {
            //'['
            RevertPoint revertPoint = CreateRevertPoint();
            if (!TryGetNextToken(out char token) || token != '[')
            {
                Revert(revertPoint);
                errorMsg = unexpectedTokenErrorMsg + " '['.";
                return false;
            }
            
            //E
            if (!E())
            {
                return false;
            }
            
            //?'
            revertPoint = CreateRevertPoint();
            if (!TryGetNextToken(out token) || token != '?')
            {
                Revert(revertPoint);
                errorMsg = unexpectedTokenErrorMsg + " '?'.";
                return false;
            }
            
            //C{C}
            char nextToken;
            do
            {
                if (!C())
                {
                    return false;
                }
                
                if (!TryLookAHead(out nextToken))
                {
                    errorMsg = unexpectedTokenErrorMsg + " '[' or '{' or a small letter or '<' or '>' or ':' or ']'.";
                    return false;
                }
                
            } while (nextToken == '[' || nextToken == '{' || IsSmallLetter(nextToken) || nextToken == '<' || nextToken == '>');
            
            revertPoint = CreateRevertPoint();
            if (!TryGetNextToken(out token))
            {
                Revert(revertPoint);
                errorMsg = unexpectedTokenErrorMsg + " ':' or ']'.";
                return false;
            }
            
            //':' or ']'
            if (token == ':')
            {
                //C{C}
                do
                {
                    if (!C())
                    {
                        return false;
                    }
                
                    if (!TryLookAHead(out nextToken))
                    {
                        errorMsg = unexpectedTokenErrorMsg + " '[' or '{' or a small letter or '<' or '>' or ':' or ']'.";
                        return false;
                    }
                
                } while (nextToken == '[' || nextToken == '{' || IsSmallLetter(nextToken) || nextToken == '<' || nextToken == '>');
                
                //']'
                revertPoint = CreateRevertPoint();
                if (token != ']')
                {
                    Revert(revertPoint);
                    errorMsg = unexpectedTokenErrorMsg + " ']'.";
                    return false;
                }
            }
            else if (token != ']')
            {
                Revert(revertPoint);
                errorMsg = unexpectedTokenErrorMsg + " ':' or ']'.";
                return false;
            }

            return true;
        }
        
        private bool W()
        {
            //'{'
            RevertPoint revertPoint = CreateRevertPoint();
            if (!TryGetNextToken(out char token) && token != '{')
            {
                Revert(revertPoint);
                errorMsg = unexpectedTokenErrorMsg + " '{'.";
                return false;
            }
            
            //E
            if (!E())
            {
                return false;
            }

            //'?'
            revertPoint = CreateRevertPoint();
            if (!TryGetNextToken(out token) || token != '?')
            {
                Revert(revertPoint);
                errorMsg = unexpectedTokenErrorMsg + " '?'.";
                return false;
            }
            
            //C{C}
            char nextToken;
            do
            {
                if (!C())
                {
                    return false;
                }
                
                if (!TryLookAHead(out nextToken))
                {
                    errorMsg = unexpectedTokenErrorMsg + " '[' or '{' or a small letter or '<' or '>' or ':' or ']'.";
                    return false;
                }
                
            } while (nextToken == '[' || nextToken == '{' || IsSmallLetter(nextToken) || nextToken == '<' || nextToken == '>');

            //'}'
            revertPoint = CreateRevertPoint();
            if (!TryGetNextToken(out token) || token != '}')
            {
                Revert(revertPoint);
                errorMsg = unexpectedTokenErrorMsg + " '}'.";
                return false;
            }

            return true;
        }

        private bool A()
        {
            //K
            RevertPoint revertPoint = CreateRevertPoint();
            if (!TryGetNextToken(out char token) || !IsSmallLetter(token))
            {
                Revert(revertPoint);
                errorMsg = unexpectedTokenErrorMsg + " a small letter.";
                return false;
            }

            //'='
            revertPoint = CreateRevertPoint();
            if (!TryGetNextToken(out token) || token != '=')
            {
                Revert(revertPoint);
                errorMsg = unexpectedTokenErrorMsg + " '='.";
                return false;
            }
            
            //E
            if (!E())
            {
                return false;
            }

            //';'
            revertPoint = CreateRevertPoint();
            if (!TryGetNextToken(out token) || token != ';')
            {
                Revert(revertPoint);
                errorMsg = unexpectedTokenErrorMsg + " ';'.";
                return false;
            }

            return true;
        }

        private bool Ç()
        {
            //'<'
            RevertPoint revertPoint = CreateRevertPoint();
            if (!TryGetNextToken(out char token) || token != '<')
            {
                Revert(revertPoint);
                errorMsg = unexpectedTokenErrorMsg + " '<'.";
                return false;
            }

            if (!E())
            {
                return false;
            }

            //';'
            revertPoint = CreateRevertPoint();
            if (!TryGetNextToken(out token) || token != ';')
            {
                Revert(revertPoint);
                errorMsg = unexpectedTokenErrorMsg + " ';'.";
                return false;
            }

            return true;
        }

        private bool G()
        {
            //'>'
            RevertPoint revertPoint = CreateRevertPoint();
            if (!TryGetNextToken(out char token) || token != '>')
            {
                Revert(revertPoint);
                errorMsg = unexpectedTokenErrorMsg + " '>'.";
                return false;
            }

            //K
            revertPoint = CreateRevertPoint();
            if (!TryGetNextToken(out token) || !IsSmallLetter(token))
            {
                Revert(revertPoint);
                errorMsg = unexpectedTokenErrorMsg + " a small letter.";
                return false;
            }
            
            //';'
            revertPoint = CreateRevertPoint();
            if (!TryGetNextToken(out token) || token != ';')
            {
                Revert(revertPoint);
                errorMsg = unexpectedTokenErrorMsg + " ';'.";
                return false;
            }

            return true;
        }
    
        private bool E()
        {
            //T
            if (!T())
            {
                return false;
            }
            
            if (TryLookAHead(out char nextToken))
            {
                errorMsg = unexpectedTokenErrorMsg + " unknown.";
                return false;
            }
            
            //{('+' | '-') T}
            while (nextToken == '+' || nextToken == '-')
            {
                //We know next token exist and next token is '+' or '-' so straight get next token
                TryGetNextToken(out char token);
                
                //T
                if (!T())
                {
                    return false;
                }
                
                if (TryLookAHead(out nextToken))
                {
                    errorMsg = unexpectedTokenErrorMsg + " unknown.";
                    return false;
                }
            }

            return true;
        }
    
        private bool T()
        {
            //U
            if (!U())
            {
                return false;
            }
            
            if (TryLookAHead(out char nextToken))
            {
                errorMsg = unexpectedTokenErrorMsg + " unknown.";
                return false;
            }
            
            //{('*' | '/' | '%') U}
            while (nextToken == '*' || nextToken == '/' || nextToken == '%')
            {
                //We know next token exist and next token is '*' or '/' or '%' so straight get next token
                TryGetNextToken(out char token);
                
                //U
                if (!U())
                {
                    return false;
                }
                
                if (TryLookAHead(out nextToken))
                {
                    errorMsg = unexpectedTokenErrorMsg + " unknown.";
                    return false;
                }
            }

            return true;
        }
    
        private bool U()
        {
            //F
            if (!F())
            {
                return false;
            }
            
            //'^' or none
            if (TryLookAHead(out char nextToken) && nextToken == '^')
            {
                //We know next token exist and next token is '^' so straight get next token
                TryGetNextToken(out char token);
                
                //'U'
                if (!U())
                {
                    return false;
                }
            }

            return true;
        }
    
        private bool F()
        {
            RevertPoint revertPoint = CreateRevertPoint();
            if (!TryGetNextToken(out char token))
            {
                Revert(revertPoint);
                errorMsg = unexpectedTokenErrorMsg + " '(' or a small letter or a numeral.";
                return false;
            }
            
            //'(' or K or R 
            if (token == '(')
            {
                //E
                if (!E())
                {
                    return false;
                }

                //')'
                revertPoint = CreateRevertPoint();
                if (!TryGetNextToken(out token) || token != ')')
                {
                    Revert(revertPoint);
                    errorMsg = unexpectedTokenErrorMsg + " ')'.";
                    return false;
                }
            }
            else if (!IsSmallLetter(token) && !IsNumber(token))
            {
                Revert(revertPoint);
                errorMsg = unexpectedTokenErrorMsg + " '(' or a small letter or a numeral.";
                return false;
            }

            return true;
        }

        private bool IsSmallLetter(char token)
        {
            for (int i = 0; i < smallCharacters.Length; i++)
            {
                if (token == smallCharacters[i]) return true;
            }
    
            return false;
        }
        
        private bool IsNumber(char token)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (token == numbers[i]) return true;
            }
    
            return false;
        }
    
        private bool TryGetNextToken(out char c)
        {
            do
            {
                if (++_charIndex >= _sourceCode[_lineIndex].Length)
                {
                    do
                    {
                        if (++_lineIndex >= _sourceCode.Count)
                        {
                            errorMsg = unexpectedTokenErrorMsg;
                            c = ' ';
                            --_lineIndex;
                            return false;
                        }
                    } while (_sourceCode[_lineIndex].Length == 0);

    
                    _charIndex = 0;
                }
                
                c = _sourceCode[_lineIndex][_charIndex];
            } while (c == ' ');
    
            return true;
        }

        private bool TryLookAHead(out char c)
        {
            RevertPoint revertPoint = CreateRevertPoint();
            bool result;
            result = TryGetNextToken(out c);
            Revert(revertPoint);
            return result;
        }
        
        private RevertPoint CreateRevertPoint()
        {
            return new RevertPoint(_lineIndex, _charIndex);
        }

        private void Revert(RevertPoint revertPoint)
        {
            _lineIndex = revertPoint.lineIndex;
            _charIndex = revertPoint.charIndex;
        }

        private void Reset()
        {
            _lineIndex = 0;
            _charIndex = -1;
        }

        private struct RevertPoint
        {
            public int lineIndex;
            public int charIndex;

            public RevertPoint(int lineIndex, int charIndex)
            {
                this.lineIndex = lineIndex;
                this.charIndex = charIndex;
            }
        }
    }
}