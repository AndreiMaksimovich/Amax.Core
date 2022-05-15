// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System.Collections.Generic;
using System.Text;
using UnityEngine;

#endregion

namespace Amax
{
    public static class CsvUtils
    {
        
        private const char QuoteChar = '"';
        private const char NewLineChar = '\n';
        private const char DefaultDelimiterChar = ',';
        private const char CarriageReturnChar = '\r';

        public static CsvFileData Read(TextAsset textAsset, char delimiter = DefaultDelimiterChar)
            => Read(textAsset.text, delimiter);

        public static CsvFileData Read(string text, char delimiter = DefaultDelimiterChar)
            => new CsvReader(text, delimiter).Read();

        private class CsvReader
        {
            
            private int _index = 0;
            private readonly CsvFileData _csvFile = new CsvFileData();
            private List<string> _row = new List<string>();
            private EReadState _state = EReadState.None;
            private readonly StringBuilder _stringBuilder = new StringBuilder();
            private readonly string _text;
            private readonly char _delimiter;
            
            public CsvReader(string text, char delimiter = DefaultDelimiterChar)
            {
                _text = text;
                _delimiter = delimiter;
            }

            public CsvFileData Read()
            {
                while (_index < _text.Length)
                {
                    var value = _text[_index];

                    // Delimiter
                    if (value == _delimiter)
                    {
                        if (_state == EReadState.QuotedValueJustRead)
                        {
                            _state = EReadState.None;
                            _index++;
                            continue;
                        }
                        if (_state != EReadState.ReadingQuotedValue)
                        {
                            FlushValue();
                            _index++;
                            continue;
                        }
                    }
                    // New Line
                    else if (value == NewLineChar)
                    {
                        if (_state != EReadState.ReadingQuotedValue)
                        {
                            FlushValue();
                            FlushRow();
                            _index++;
                            continue;
                        }
                    }
                    // Quote
                    else if (value == QuoteChar)
                    {
                        if (_state == EReadState.None)
                        {
                            _state = EReadState.ReadingQuotedValue;
                            _index++;
                            continue;
                        }
                        if (_state == EReadState.ReadingQuotedValue)
                        {
                            if (IsInRange(_index + 1) && _text[_index + 1] == QuoteChar)
                            {
                                _stringBuilder.Append(QuoteChar);
                                _index += 2;
                                continue;
                            }
                            else
                            {
                                FlushValue();
                                _state = EReadState.QuotedValueJustRead;
                                _index++;
                                continue;
                            }
                        }
                    }

                    if (_state == EReadState.None)
                    {
                        _state = EReadState.ReadingValue;
                    }
                    
                    if (value!=CarriageReturnChar) _stringBuilder.Append(value);
                    
                    _index++;

                }
                
                if (_state == EReadState.ReadingValue) FlushValue();
                FlushRow();

                return _csvFile;

            }

            private bool IsInRange(int index) => _text.Length > index;

            private void FlushValue()
            {
                _state = EReadState.None;
                _row.Add(_stringBuilder.ToString());
                _stringBuilder.Clear();
            }

            private void FlushRow()
            {
                _state = EReadState.None;
                _csvFile.Data.Add(_row);
                _row = new List<string>();
            }
            
            private enum EReadState
            {
                None, ReadingValue, ReadingQuotedValue, QuotedValueJustRead
            }
            
        }
        
    }

    public sealed class CsvFileData
    {
        public List<List<string>> Data { get; } = new List<List<string>>();
        public string this[int row, int column] => Data[row][column];
        public List<string> this[int row] => Data[row];
        public int RowCount => Data.Count;
        public List<string> HeaderRow => Data.Count > 0 ? Data[0] : null;
    }
    
}