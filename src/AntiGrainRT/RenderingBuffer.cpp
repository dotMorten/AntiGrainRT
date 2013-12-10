#include "pch.h"
#include "RenderingBuffer.h"

using namespace Windows::Storage::Streams;
using namespace concurrency;
using namespace AntiGrainRT;
using namespace Platform;

void RenderingBuffer::SetPixel(unsigned int x, unsigned int y, Windows::UI::Color color)
{
	//	// Get the pointer to the beginning of the i-th row (Y-coordinate)
	//	// and shift it to the i-th position, that is, X-coordinate.
	unsigned char* ptr = m_rbuf.row_ptr(y) + x * 3;
	*ptr++ = color.R; // R
	*ptr++ = color.G; // G
	*ptr++ = color.B;  // B
}

RenderingBuffer::RenderingBuffer(unsigned int width, unsigned int height) : m_width(width), m_height(height)
{
	m_buffer = ref new ::Platform::Array<uint8>(width * height * 3);

	memset(m_buffer->Data, 255, width * height * 3);

	agg::rendering_buffer rbuf(m_buffer->Data,
		width,
		height,
		width * 3);
	m_rbuf = rbuf;

	//for (i = 0; i < rbuf.height(); ++i)
	//{
	//	unsigned char* p = rbuf.row_ptr(i);
	//	*p++ = 0; *p++ = 0; *p++ = 0;
	//	p += (rbuf.width() - 2) * 3;
	//	*p++ = 0; *p++ = 0; *p++ = 0;
	//}
	//memset(rbuf.row_ptr(0), 0, rbuf.width() * 3);
	//memset(rbuf.row_ptr(rbuf.height() - 1), 0, rbuf.width() * 3);


	//draw_black_frame(rbuf);
}

Windows::Foundation::IAsyncAction^ RenderingBuffer::SavePpm(Windows::Storage::IStorageFile^ file)
{
	//auto path = Windows::Storage::ApplicationData::Current->TemporaryFolder->Path + "\\out.ppm";
	//FILE* fd;
	//auto err = fopen_s(&fd, file->Path->Data(), "wb");
	//if (fd)
	//{
	//	fprintf(fd, "P6 %d %d 255 ", m_width, m_height);
	//	fwrite(m_buffer->Data, 1, m_buffer->Length, fd);
	//	fclose(fd);
	//}
	//auto header = ref new Platform::String("P6 " + m_width + " " + m_height + " 255 ");
	auto buffer = m_buffer;
	//auto op = create_async([file, header](void) { Windows::Storage::FileIO::WriteTextAsync(file, header); })
    //                 .then([file, buffer]() { return Windows::Storage::FileIO::WriteBytesAsync(file, buffer); });
	//return op;
	auto result_async_operation =
		create_async([file, buffer](void)
	{
	//	auto stream = ref new InMemoryRandomAccessStream();
	//	auto openTask = create_task(file->OpenAsync(Windows::Storage::FileAccessMode::ReadWrite))
	//		.then([data](IRandomAccessStream^ stream)
	//	{
	//		stream->Seek(0);
	//		stream->WriteAsync()
	//	});
	//	return openTask;
	});
	return result_async_operation;
}
