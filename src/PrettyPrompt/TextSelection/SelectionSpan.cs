﻿#region License Header
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.
#endregion

using PrettyPrompt.Consoles;
using PrettyPrompt.Documents;
using System.Collections.Generic;

namespace PrettyPrompt.TextSelection
{
    internal class SelectionSpan
    {
        public SelectionSpan(ConsoleCoordinate anchor, ConsoleCoordinate cursor)
        {
            Anchor = anchor;
            Cursor = cursor;
        }

        public ConsoleCoordinate Anchor { get; set; }

        public ConsoleCoordinate Cursor { get; set; }

        public ConsoleCoordinate Start => IsCursorPastAnchor() ? Anchor : Cursor;

        public ConsoleCoordinate End => IsCursorPastAnchor() ? Cursor : Anchor;

        private bool IsCursorPastAnchor() =>
            Anchor.Row < Cursor.Row || ((Anchor.Row == Cursor.Row) && Anchor.Column < Cursor.Column);

        public (int start, int end) GetCaretIndices(IReadOnlyList<WrappedLine> wrappedLines)
        {
            var start = Start;
            var end = End;
            var selectionStart = wrappedLines[start.Row].StartIndex + start.Column;
            var selectionEnd = wrappedLines[end.Row].StartIndex + end.Column + 1;
            if(end.Column == wrappedLines[end.Row].Content.Length)
            {
                selectionEnd--;
            }
            return (selectionStart, selectionEnd);
        }
    }
}