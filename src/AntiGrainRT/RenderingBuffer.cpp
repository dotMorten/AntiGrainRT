#include "pch.h"
#include "RenderingBuffer.h"

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Storage::Streams;
using namespace concurrency;
using namespace AntiGrainRT;
using namespace concurrency;
#if (WINAPI_FAMILY==WINAPI_FAMILY_APP)
using namespace Windows::UI::Xaml::Media;
using namespace Windows::Graphics::Imaging;
using namespace Windows::UI::Xaml::Media::Imaging;
#endif

void RenderingBuffer::SetPixel(unsigned int x, unsigned int y, Windows::UI::Color color)
{
	//	// Get the pointer to the beginning of the i-th row (Y-coordinate)
	//	// and shift it to the i-th position, that is, X-coordinate.
	auto ptr = reinterpret_cast<unsigned int*>(m_rbuf.row_ptr(y) + x * 4);
  auto pColor = reinterpret_cast<unsigned char*>(&color);
  if (m_format == BitmapPixelFormat::Rgba8) 
  {
    *ptr = (pColor[1] << 0) | (pColor[2] << 8) | (pColor[3] << 16) | (pColor[0] << 24);
  }
  else if (m_format == BitmapPixelFormat::Bgra8)
  {
    *ptr = (pColor[3] << 0) | (pColor[2] << 8) | (pColor[1] << 16) | (pColor[0] << 24);
  }
}

Windows::UI::Color RenderingBuffer::GetPixel(unsigned int x, unsigned int y)
{
  unsigned char* ptr = m_rbuf.row_ptr(y) + x * 4;
  Windows::UI::Color color;
  if (m_format == BitmapPixelFormat::Rgba8)
  {
    color.R = *ptr++;
    color.G = *ptr++;
    color.B = *ptr++;
    color.A = *ptr++;
  }
  else if (m_format == BitmapPixelFormat::Bgra8)
  {
    color.B = *ptr++;
    color.G = *ptr++;
    color.R = *ptr++;
    color.A = *ptr++;
  }
  return color;
}

RenderingBuffer::RenderingBuffer(unsigned int width, unsigned int height, BitmapPixelFormat format)
: m_width(width), m_height(height), m_format(format)
{
  
  
	m_buffer = ref new ::Platform::Array<uint8>(width * height * 4);

	memset(m_buffer->Data, 255, width * height * 4);

	agg::rendering_buffer rbuf(m_buffer->Data,
		width,
		height,
		width * 4);
	m_rbuf = rbuf;
}

//Windows::Foundation::IAsyncAction^ RenderingBuffer::SavePpmAsync(Windows::Storage::IStorageFile^ file)
//{
//	auto result_async_operation =
//		create_async([=](void)
//	{
//    auto stream = create_task(file->OpenAsync(Windows::Storage::FileAccessMode::ReadWrite)).get();
//    auto outstream = stream->GetOutputStreamAt(0);
//    DataWriter^ writer = ref new DataWriter(outstream);
//    writer->WriteString("P6 " + m_width + " " + m_height + " 255 ");
//    writer->WriteBytes(m_buffer);
//    writer->StoreAsync(); //TODO: This is not awaited
//	});
//	return result_async_operation;
//}

#if (WINAPI_FAMILY==WINAPI_FAMILY_APP)

IAsyncOperation<ImageSource^>^ RenderingBuffer::CreateImageSourceAsync()
{
  auto buffer = m_buffer;
  // we need to return an IAsyncOperation, so use create_async to create that code in a lambda
  auto result_async_operation =
    create_async([=](void)
  {
    // This will be the result bitmap, we create it here so it gets captured in lambda later on
    auto bitmap = ref new BitmapImage();
    auto stream = ref new InMemoryRandomAccessStream();

    // Note: used chained thens to ensure we can access the original calling context (the ui threadtypicall) at the end of the async sequence

    // #1 create an encoder referencing the the empty stream 
    auto task = create_task(BitmapEncoder::CreateAsync(BitmapEncoder::PngEncoderId, stream))

      .then([=](BitmapEncoder^ encoder)
    {
      // #2 render pixel data into a byte array on a background thread
      Array<unsigned char>^ pixels = buffer;
      encoder->SetPixelData(BitmapPixelFormat::Bgra8, BitmapAlphaMode::Straight, m_width, m_height, 96, 96, pixels);
      return encoder->FlushAsync();
    }, task_continuation_context::use_arbitrary()) // run the above code on the thread pool (encoder are MTA objects so this is ok)

      .then([stream, bitmap](void)
    {
      // #3 push the stream into the result bitmap, do this on the ui thread
      stream->Seek(0);
      return bitmap->SetSourceAsync(stream);
    }) /* don't use thread pool - use ui (original calling context)*/

      .then([bitmap](void)
    {
      // #4 wait for SetSourceAsync to finish, then return the bitmap as an image source
      auto image_source = static_cast<Windows::UI::Xaml::Media::ImageSource^>(bitmap);
      return image_source;
    });
    return task;
  });
  return result_async_operation;
}
#endif
