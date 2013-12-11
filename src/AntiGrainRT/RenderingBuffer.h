#pragma once

namespace AntiGrainRT
{
  public ref class RenderingBuffer sealed
  {
    public:
      RenderingBuffer(unsigned int width, unsigned int height, Windows::Graphics::Imaging::BitmapPixelFormat format);
      void SetPixel(unsigned int x, unsigned int y, Windows::UI::Color color);
      Windows::UI::Color GetPixel(unsigned int x, unsigned int y);
      //Windows::Foundation::IAsyncAction^ SavePpmAsync(Windows::Storage::IStorageFile^ file);
#if (WINAPI_FAMILY==WINAPI_FAMILY_APP)
      Windows::Foundation::IAsyncOperation<Windows::UI::Xaml::Media::ImageSource^>^ CreateImageSourceAsync();
#endif
      property unsigned int PixelWidth { unsigned int get() { return m_width; } }
      property unsigned int PixelHeight { unsigned int get() { return m_height; } }
      property Windows::Graphics::Imaging::BitmapPixelFormat PixelFormat { Windows::Graphics::Imaging::BitmapPixelFormat get() { return m_format; } }
    private:
      agg::rendering_buffer m_rbuf;
      Platform::Array<uint8>^ m_buffer;
      unsigned int m_width;
      unsigned int m_height;
      Windows::Graphics::Imaging::BitmapPixelFormat m_format;
  };
}