using System;
using System.Collections.Generic;

namespace Compiler
{
    public class Compiler
    {
        private const string unexpectedTokenErrorMsg = "Encountered with an unexpected token";
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
            else Console.WriteLine(errorMsg);
        }
        
        private bool P()
        {
            //{C}
            while (C());

            if (!TryGetNextToken(out char token))
            {
                return false;
            }
            
            //'.'
            if (token != '.')
            {
                return false;
            }

            return true;
        }
    
        private bool C()
        {
            //I or W or A or Ç or G
            if (!I() && !W() && !A() && !Ç() && G())
            {
                return false;
            }

            return true;
        }

        private bool I()
        {
            if (!TryGetNextToken(out char token))
            {
                return false;
            }
            
            //'['
            if (token != '[')
            {
                return false;
            }
            
            //E
            if (!E())
            {
                return false;
            }
                
            if (!TryGetNextToken(out token))
            {
                return false;
            }
            
            //?'
            if (token != '?')
            {
                return false;
            }
            
            //C
            if (!C())
            {
                return false;
            }
            
            //{C}
            while (C());
            
            if (!TryGetNextToken(out token))
            {
                return false;
            }
            
            //':' or ']'
            if (token == ':')
            {
                //C
                if (!C())
                {
                    return false;
                }
                
                //{C}
                while (C());
                
                //']'
                if (token != ']')
                {
                    return false;
                }
            }
            else if (token != ']')
            {
                return false;
            }

            return true;
        }
        
        private bool W()
        {
            if (!TryGetNextToken(out char token))
            {
                return false;
            }
            
            //'{'
            if (token != '{')
            {
                return false;
            }
            
            //E
            if (!E())
            {
                return false;
            }
            
            if (!TryGetNextToken(out token))
            {
                return false;
            }
            
            //'?'
            if (token != '?')
            {
                return false;
            }
            
            //C
            if (!C())
            {
                return false;
            }
            
            //{C}
            while (C());

            //'}'
            if (token != '}')
            {
                return false;
            }

            return true;
        }

        private bool A()
        {
            if (!TryGetNextToken(out char token))
            {
                return false;
            }
            
            //K
            if (!IsSmallCharacter(token))
            {
                return false;
            }
            
            if (!TryGetNextToken(out token))
            {
                return false;
            }
            
            //'='
            if (token != '=')
            {
                return false;
            }
            
            //E
            if (!E())
            {
                return false;
            }
            
            if (!TryGetNextToken(out token))
            {
                return false;
            }
            
            //';'
            if (token != ';')
            {
                return false;
            }

            return true;
        }

        private bool Ç()
        {
            if (!TryGetNextToken(out char token))
            {
                return false;
            }
            
            //'<'
            if (token != '<')
            {
                return false;
            }

            if (!E())
            {
                return false;
            }
            
            if (!TryGetNextToken(out token))
            {
                return false;
            }
            
            //';'
            if (token != ';')
            {
                return false;
            }

            return true;
        }

        private bool G()
        {
            if (!TryGetNextToken(out char token))
            {
                return false;
            }
            
            //'>'
            if (token != '>')
            {
                return false;
            }
            
            if (!TryGetNextToken(out token))
            {
                return false;
            }
            
            //K
            if (!IsSmallCharacter(token))
            {
                return false;
            }
            
            if (!TryGetNextToken(out token))
            {
                return false;
            }
            
            //';'
            if (token != ';')
            {
                return false;
            }

            return true;
        }
    
        private bool E()
        {
            //'T'
            if (!T())
            {
                return false;
            }
            
            //{('+' | '-') T}
            while (TryLookAHead(out char nextToken) && (nextToken == '+' || nextToken == '-'))
            {
                //We know next token exist and next token is '+' or '-' so straight get next token
                TryGetNextToken(out char token);
                
                //'T'
                if (!T())
                {
                    return false;
                }
            }

            return true;
        }
    
        private bool T()
        {
            //'U'
            if (!U())
            {
                return false;
            }

            //'{('*' | '/' | '%') U}'
            while (TryLookAHead(out char nextToken) && (nextToken == '*' || nextToken == '/' || nextToken == '%'))
            {
                //We know next token exist and next token is '*' or '/' pr '%' so straight get next token
                TryGetNextToken(out char token);
                
                //U
                if (!U())
                {
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

            if (!TryLookAHead(out char nextToken))
            {
                return false;
            }
            
            //'^' or none
            if (nextToken == '^')
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
            if (!TryGetNextToken(out char token))
            {
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

                if (!TryGetNextToken(out token))
                {
                    return false;
                }
                
                //')'
                if (token != ')')
                {
                    return false;
                }
            }
            else if (!IsSmallCharacter(token) && !IsNumber(token))
            {
                return false;
            }

            return true;
        }

        private bool IsSmallCharacter(char token)
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
                    if (++_lineIndex > _sourceCode.Count)
                    {
                        Error(unexpectedTokenErrorMsg);
                        c = ' ';
                        return false;
                    }
    
                    _charIndex = 0;
                }
                
                c = _sourceCode[_lineIndex][_charIndex];
            } while (c == ' ');
    
            return true;
        }

        private bool TryLookAHead(out char c)
        {
            var rememberCharIndex = _charIndex;
            var rememberLineIndex = _lineIndex;
            
            if (!TryGetNextToken(out c)) return false;

            _charIndex = rememberCharIndex;
            _lineIndex = rememberLineIndex;
            return true;
        }
        
        private void OneStepBack()
        {
            if (--_charIndex < 0)
            {
                _charIndex = _sourceCode[--_lineIndex].Length;
            }
        }
    
        private void Error(string errorStr)
        {
            Console.WriteLine(errorStr);
        }
        
        private void Reset()
        {
            _lineIndex = 0;
            _charIndex = -1;
        }
    }
}