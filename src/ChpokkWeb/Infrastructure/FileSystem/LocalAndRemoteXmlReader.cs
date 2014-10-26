using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Schema;

namespace ChpokkWeb.Infrastructure.FileSystem {
	public class LocalAndRemoteXmlReader:XmlReader {
		public LocalAndRemoteXmlReader(string url) {
			_inner = new XmlTextReader(url);
		}

		public override object ReadContentAsObject() {
			return _inner.ReadContentAsObject();
		}

		public override string LookupNamespace(string prefix) {
			return _inner.LookupNamespace(prefix);
		}

		public override void ResolveEntity() {
			_inner.ResolveEntity();
		}

		public override string GetAttribute(string name) {
			return _inner.GetAttribute(name);
		}

		public override string GetAttribute(string localName, string namespaceURI) {
			return _inner.GetAttribute(localName, namespaceURI);
		}

		public override string GetAttribute(int i) {
			return _inner.GetAttribute(i);
		}

		public override bool MoveToAttribute(string name) {
			return _inner.MoveToAttribute(name);
		}

		public override bool MoveToAttribute(string localName, string namespaceURI) {
			return _inner.MoveToAttribute(localName, namespaceURI);
		}


		public override bool MoveToFirstAttribute() {
			return _inner.MoveToFirstAttribute();
		}

		public override bool MoveToNextAttribute() {
			return _inner.MoveToNextAttribute();
		}

		public override bool MoveToElement() {
			return _inner.MoveToElement();
		}

		public override bool ReadAttributeValue() {
			return _inner.ReadAttributeValue();
		}

		public override bool Read() {
			return _inner.Read();
		}

		public override bool ReadContentAsBoolean() {
			return _inner.ReadContentAsBoolean();
		}

		public override DateTime ReadContentAsDateTime() {
			return _inner.ReadContentAsDateTime();
		}

		public override DateTimeOffset ReadContentAsDateTimeOffset() {
			return _inner.ReadContentAsDateTimeOffset();
		}

		public override double ReadContentAsDouble() {
			return _inner.ReadContentAsDouble();
		}

		public override float ReadContentAsFloat() {
			return _inner.ReadContentAsFloat();
		}

		public override decimal ReadContentAsDecimal() {
			return _inner.ReadContentAsDecimal();
		}

		public override int ReadContentAsInt() {
			return _inner.ReadContentAsInt();
		}

		public override long ReadContentAsLong() {
			return _inner.ReadContentAsLong();
		}

		public override string ReadContentAsString() {
			return _inner.ReadContentAsString();
		}

		public override object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver) {
			return _inner.ReadContentAs(returnType, namespaceResolver);
		}

		public override object ReadElementContentAsObject() {
			return _inner.ReadElementContentAsObject();
		}

		public override object ReadElementContentAsObject(string localName, string namespaceURI) {
			return _inner.ReadElementContentAsObject(localName, namespaceURI);
		}

		public override bool ReadElementContentAsBoolean() {
			return _inner.ReadElementContentAsBoolean();
		}

		public override bool ReadElementContentAsBoolean(string localName, string namespaceURI) {
			return _inner.ReadElementContentAsBoolean(localName, namespaceURI);
		}

		public override DateTime ReadElementContentAsDateTime() {
			return _inner.ReadElementContentAsDateTime();
		}

		public override DateTime ReadElementContentAsDateTime(string localName, string namespaceURI) {
			return _inner.ReadElementContentAsDateTime(localName, namespaceURI);
		}

		public override double ReadElementContentAsDouble() {
			return _inner.ReadElementContentAsDouble();
		}

		public override double ReadElementContentAsDouble(string localName, string namespaceURI) {
			return _inner.ReadElementContentAsDouble(localName, namespaceURI);
		}

		public override float ReadElementContentAsFloat() {
			return _inner.ReadElementContentAsFloat();
		}

		public override float ReadElementContentAsFloat(string localName, string namespaceURI) {
			return _inner.ReadElementContentAsFloat(localName, namespaceURI);
		}

		public override decimal ReadElementContentAsDecimal() {
			return _inner.ReadElementContentAsDecimal();
		}

		public override decimal ReadElementContentAsDecimal(string localName, string namespaceURI) {
			return _inner.ReadElementContentAsDecimal(localName, namespaceURI);
		}

		public override int ReadElementContentAsInt() {
			return _inner.ReadElementContentAsInt();
		}

		public override int ReadElementContentAsInt(string localName, string namespaceURI) {
			return _inner.ReadElementContentAsInt(localName, namespaceURI);
		}

		public override long ReadElementContentAsLong() {
			return _inner.ReadElementContentAsLong();
		}

		public override long ReadElementContentAsLong(string localName, string namespaceURI) {
			return _inner.ReadElementContentAsLong(localName, namespaceURI);
		}

		public override string ReadElementContentAsString() {
			return _inner.ReadElementContentAsString();
		}

		public override string ReadElementContentAsString(string localName, string namespaceURI) {
			return _inner.ReadElementContentAsString(localName, namespaceURI);
		}

		public override object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver) {
			return _inner.ReadElementContentAs(returnType, namespaceResolver);
		}

		public override object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver, string localName,
		                                   string namespaceURI) {
			return _inner.ReadElementContentAs(returnType, namespaceResolver, localName, namespaceURI);
		}

		public override int ReadValueChunk(char[] buffer, int index, int count) {
			return _inner.ReadValueChunk(buffer, index, count);
		}

		public override XmlNodeType MoveToContent() {
			return _inner.MoveToContent();
		}

		public override void ReadStartElement() {
			_inner.ReadStartElement();
		}

		public override void ReadStartElement(string name) {
			_inner.ReadStartElement(name);
		}

		public override void ReadStartElement(string localname, string ns) {
			_inner.ReadStartElement(localname, ns);
		}

		public override string ReadElementString() {
			return _inner.ReadElementString();
		}

		public override string ReadElementString(string name) {
			return _inner.ReadElementString(name);
		}

		public override string ReadElementString(string localname, string ns) {
			return _inner.ReadElementString(localname, ns);
		}

		public override void ReadEndElement() {
			_inner.ReadEndElement();
		}

		public override bool IsStartElement() {
			return _inner.IsStartElement();
		}

		public override bool IsStartElement(string name) {
			return _inner.IsStartElement(name);
		}

		public override bool IsStartElement(string localname, string ns) {
			return _inner.IsStartElement(localname, ns);
		}

		public override bool ReadToFollowing(string name) {
			return _inner.ReadToFollowing(name);
		}

		public override bool ReadToFollowing(string localName, string namespaceURI) {
			return _inner.ReadToFollowing(localName, namespaceURI);
		}

		public override bool ReadToDescendant(string name) {
			return _inner.ReadToDescendant(name);
		}

		public override bool ReadToDescendant(string localName, string namespaceURI) {
			return _inner.ReadToDescendant(localName, namespaceURI);
		}

		public override bool ReadToNextSibling(string name) {
			return _inner.ReadToNextSibling(name);
		}

		public override bool ReadToNextSibling(string localName, string namespaceURI) {
			return _inner.ReadToNextSibling(localName, namespaceURI);
		}

		public override string ReadInnerXml() {
			return _inner.ReadInnerXml();
		}

		public override string ReadOuterXml() {
			return _inner.ReadOuterXml();
		}

		public override XmlReader ReadSubtree() {
			return _inner.ReadSubtree();
		}

		public void Dispose() {
			_inner.Dispose();
		}

		public override Task<string> GetValueAsync() {
			return _inner.GetValueAsync();
		}

		public override Task<object> ReadContentAsObjectAsync() {
			return _inner.ReadContentAsObjectAsync();
		}

		public override Task<string> ReadContentAsStringAsync() {
			return _inner.ReadContentAsStringAsync();
		}

		public override Task<object> ReadContentAsAsync(Type returnType, IXmlNamespaceResolver namespaceResolver) {
			return _inner.ReadContentAsAsync(returnType, namespaceResolver);
		}

		public override Task<object> ReadElementContentAsObjectAsync() {
			return _inner.ReadElementContentAsObjectAsync();
		}

		public override Task<string> ReadElementContentAsStringAsync() {
			return _inner.ReadElementContentAsStringAsync();
		}

		public override Task<object> ReadElementContentAsAsync(Type returnType, IXmlNamespaceResolver namespaceResolver) {
			return _inner.ReadElementContentAsAsync(returnType, namespaceResolver);
		}

		public override Task<bool> ReadAsync() {
			return _inner.ReadAsync();
		}

		public override Task SkipAsync() {
			return _inner.SkipAsync();
		}

		public override Task<int> ReadContentAsBase64Async(byte[] buffer, int index, int count) {
			return _inner.ReadContentAsBase64Async(buffer, index, count);
		}

		public override Task<int> ReadElementContentAsBase64Async(byte[] buffer, int index, int count) {
			return _inner.ReadElementContentAsBase64Async(buffer, index, count);
		}

		public override Task<int> ReadContentAsBinHexAsync(byte[] buffer, int index, int count) {
			return _inner.ReadContentAsBinHexAsync(buffer, index, count);
		}

		public override Task<int> ReadElementContentAsBinHexAsync(byte[] buffer, int index, int count) {
			return _inner.ReadElementContentAsBinHexAsync(buffer, index, count);
		}

		public override Task<int> ReadValueChunkAsync(char[] buffer, int index, int count) {
			return _inner.ReadValueChunkAsync(buffer, index, count);
		}

		public override Task<XmlNodeType> MoveToContentAsync() {
			return _inner.MoveToContentAsync();
		}

		public override Task<string> ReadInnerXmlAsync() {
			return _inner.ReadInnerXmlAsync();
		}

		public override Task<string> ReadOuterXmlAsync() {
			return _inner.ReadOuterXmlAsync();
		}

		public override XmlReaderSettings Settings {
			get { return _inner.Settings; }
		}

		public override IXmlSchemaInfo SchemaInfo {
			get { return _inner.SchemaInfo; }
		}

		public override Type ValueType {
			get { return _inner.ValueType; }
		}

		public override string this[int i] {
			get { return _inner[i]; }
		}

		public override string this[string name] {
			get { return _inner[name]; }
		}

		public override string this[string name, string namespaceURI] {
			get { return _inner[name, namespaceURI]; }
		}

		public override bool HasAttributes {
			get { return _inner.HasAttributes; }
		}

		public string LookupPrefix(string namespaceName) {
			return ((IXmlNamespaceResolver) _inner).LookupPrefix(namespaceName);
		}


		public override void MoveToAttribute(int i) {
			_inner.MoveToAttribute(i);
		}



		public override void Close() {
			_inner.Close();
		}

		public override void Skip() {
			_inner.Skip();
		}

		public override int ReadContentAsBase64(byte[] buffer, int index, int count) {
			return _inner.ReadContentAsBase64(buffer, index, count);
		}

		public override int ReadElementContentAsBase64(byte[] buffer, int index, int count) {
			return _inner.ReadElementContentAsBase64(buffer, index, count);
		}

		public override int ReadContentAsBinHex(byte[] buffer, int index, int count) {
			return _inner.ReadContentAsBinHex(buffer, index, count);
		}

		public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count) {
			return _inner.ReadElementContentAsBinHex(buffer, index, count);
		}

		public override string ReadString() {
			return _inner.ReadString();
		}

		public bool HasLineInfo() {
			return _inner.HasLineInfo();
		}

		public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope) {
			return _inner.GetNamespacesInScope(scope);
		}

		public void ResetState() {
			_inner.ResetState();
		}

		public TextReader GetRemainder() {
			return _inner.GetRemainder();
		}

		public int ReadChars(char[] buffer, int index, int count) {
			return _inner.ReadChars(buffer, index, count);
		}

		public int ReadBase64(byte[] array, int offset, int len) {
			return _inner.ReadBase64(array, offset, len);
		}

		public int ReadBinHex(byte[] array, int offset, int len) {
			return _inner.ReadBinHex(array, offset, len);
		}

		public override XmlNodeType NodeType {
			get { return _inner.NodeType; }
		}

		public override string Name {
			get { return _inner.Name; }
		}

		public override string LocalName {
			get { return _inner.LocalName; }
		}

		public override string NamespaceURI {
			get { return _inner.NamespaceURI; }
		}

		public override string Prefix {
			get { return _inner.Prefix; }
		}

		public override bool HasValue {
			get { return _inner.HasValue; }
		}

		public override string Value {
			get { return _inner.Value; }
		}

		public override int Depth {
			get { return _inner.Depth; }
		}

		public override string BaseURI {
			get { return _inner.BaseURI; }
		}

		public override bool IsEmptyElement {
			get { return _inner.IsEmptyElement; }
		}

		public override bool IsDefault {
			get { return _inner.IsDefault; }
		}

		public override char QuoteChar {
			get { return _inner.QuoteChar; }
		}

		public override XmlSpace XmlSpace {
			get { return _inner.XmlSpace; }
		}

		public override string XmlLang {
			get { return _inner.XmlLang; }
		}

		public override int AttributeCount {
			get { return _inner.AttributeCount; }
		}

		public override bool EOF {
			get { return _inner.EOF; }
		}

		public override ReadState ReadState {
			get { return _inner.ReadState; }
		}

		public override XmlNameTable NameTable {
			get { return _inner.NameTable; }
		}

		public override bool CanResolveEntity {
			get { return _inner.CanResolveEntity; }
		}

		public override bool CanReadBinaryContent {
			get { return _inner.CanReadBinaryContent; }
		}

		public override bool CanReadValueChunk {
			get { return _inner.CanReadValueChunk; }
		}

		public int LineNumber {
			get { return _inner.LineNumber; }
		}

		public int LinePosition {
			get { return _inner.LinePosition; }
		}

		public bool Namespaces {
			get { return _inner.Namespaces; }
			set { _inner.Namespaces = value; }
		}

		public bool Normalization {
			get { return _inner.Normalization; }
			set { _inner.Normalization = value; }
		}

		public Encoding Encoding {
			get { return _inner.Encoding; }
		}

		public WhitespaceHandling WhitespaceHandling {
			get { return _inner.WhitespaceHandling; }
			set { _inner.WhitespaceHandling = value; }
		}

		public bool ProhibitDtd {
			get { return _inner.ProhibitDtd; }
			set { _inner.ProhibitDtd = value; }
		}

		public DtdProcessing DtdProcessing {
			get { return _inner.DtdProcessing; }
			set { _inner.DtdProcessing = value; }
		}

		public EntityHandling EntityHandling {
			get { return _inner.EntityHandling; }
			set { _inner.EntityHandling = value; }
		}

		public XmlResolver XmlResolver {
			set { _inner.XmlResolver = value; }
		}

		private XmlTextReader _inner;
	}
}