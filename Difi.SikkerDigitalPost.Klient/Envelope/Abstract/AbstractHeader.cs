﻿/** 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Xml;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Abstract
{
    internal abstract class AbstractHeader : EnvelopeXmlPart
    {
        protected XmlNode Security;

        protected AbstractHeader(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            var header = Context.CreateElement("env", "Header", NavneromUtility.SoapEnvelopeEnv12);
            header.AppendChild(SecurityElement());
            header.AppendChild(MessagingElement());
            return header;
        }

        protected abstract XmlNode SecurityElement();

        protected abstract XmlNode MessagingElement();

        public abstract void AddSignatureElement();
    }
}
