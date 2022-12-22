using System;
using System.IO;

namespace LeadingEdge.Curator.Core.Application
{
	public class PositionWrapperStream : Stream
	{
		private readonly Stream wrapped;

		private int pos;

		public PositionWrapperStream(Stream wrapped)
		{
			this.wrapped = wrapped;
		}

		public override bool CanSeek { get { return false; } }

		public override bool CanWrite { get { return true; } }

		public override bool CanRead { get { return false; } }

		public override long Length
		{
			get { throw new NotImplementedException(); }
		}

		public override long Position
		{
			get { return pos; }
			set { throw new NotSupportedException(); }
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			pos += count;
			wrapped.Write(buffer, offset, count);
		}

		public override void Flush()
		{
			wrapped.Flush();
		}

		protected override void Dispose(bool disposing)
		{
			wrapped.Dispose();
			base.Dispose(disposing);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}
	}
}
