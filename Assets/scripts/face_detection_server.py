
# # # #yg goy tsaraigaa toirogoor haruulna
# # # from flask import Flask, request, send_file
# # # import cv2
# # # import numpy as np
# # # from PIL import Image, ImageDraw
# # # import io
# # # import math

# # # app = Flask(__name__)
# # # face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + "haarcascade_frontalface_default.xml")

# # # def make_circular(image_np):
# # #     """Зургийг дугуй хэлбэртэй болгох"""
# # #     pil_img = Image.fromarray(cv2.cvtColor(image_np, cv2.COLOR_BGR2RGB))
    
# # #     mask = Image.new('L', pil_img.size, 0)
# # #     draw = ImageDraw.Draw(mask)
# # #     draw.ellipse((0, 0, pil_img.size[0], pil_img.size[1]), fill=255)
    
# # #     result = Image.new('RGBA', pil_img.size)
# # #     result.paste(pil_img, (0, 0), mask)
    
# # #     return cv2.cvtColor(np.array(result), cv2.COLOR_RGBA2BGR)

# # # @app.route("/detect_face", methods=["POST"])
# # # def detect_face():
# # #     if "image" not in request.files:
# # #         return "No image uploaded", 400

# # #     file = request.files["image"]
    
# # #     # Файлын мэдээллийг хэвлэх
# # #     print(f"✅ Received image: {file.filename}, size: {len(file.read())} bytes")
# # #     file.seek(0)  # Файлыг дахин уншихад бэлтгэх

# # #     try:
# # #         image_bytes = file.read()
# # #         nparr = np.frombuffer(image_bytes, np.uint8)
# # #         img_np = cv2.imdecode(nparr, cv2.IMREAD_COLOR)

# # #         gray = cv2.cvtColor(img_np, cv2.COLOR_BGR2GRAY)
# # #         faces = face_cascade.detectMultiScale(gray, 1.3, 5)

# # #         if len(faces) == 0:
# # #             return "No face detected", 404

# # #         (x, y, w, h) = faces[0]
# # #         cropped = img_np[y:y+h, x:x+w]
        
# # #         # Дугуй хэлбэртэй болгох
# # #         circular_face = make_circular(cropped)

# # #         _, jpeg = cv2.imencode('.jpg', circular_face)
# # #         return send_file(
# # #             io.BytesIO(jpeg.tobytes()),
# # #             mimetype='image/jpeg',
# # #             as_attachment=True,
# # #             download_name='circular_face.jpg'
# # #         )

# # #     except Exception as e:
# # #         return f"Error: {str(e)}", 500

# # # if __name__ == "__main__":
# # #     app.run(host="0.0.0.0", port=5000)

    
# # # #######################################################################
# # # #######################################################################
# # # #######################################################################
# # # # #######################################################################
# # # from flask import Flask, request, send_file
# # # import cv2
# # # import numpy as np
# # # from PIL import Image, ImageDraw, ImageOps
# # # import io
# # # import math
# # # import exifread  # Add this import for EXIF handling

# # # app = Flask(__name__)
# # # face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + "haarcascade_frontalface_default.xml")

# # # def correct_orientation(image_np, image_bytes):
# # #     """Correct image orientation based on EXIF data"""
# # #     try:
# # #         # Create PIL Image from bytes to check EXIF
# # #         pil_img = Image.open(io.BytesIO(image_bytes))
        
# # #         # Check for EXIF orientation tag
# # #         for orientation in ExifTags.TAGS.keys():
# # #             if ExifTags.TAGS[orientation] == 'Orientation':
# # #                 break
        
# # #         exif = pil_img._getexif()
# # #         if exif is not None and orientation in exif:
# # #             if exif[orientation] == 3:
# # #                 image_np = cv2.rotate(image_np, cv2.ROTATE_180)
# # #             elif exif[orientation] == 6:
# # #                 image_np = cv2.rotate(image_np, cv2.ROTATE_90_CLOCKWISE)
# # #             elif exif[orientation] == 8:
# # #                 image_np = cv2.rotate(image_np, cv2.ROTATE_90_COUNTERCLOCKWISE)
                
# # #     except Exception as e:
# # #         print(f"Orientation correction error: {str(e)}")
    
# # #     return image_np

# # # def make_circular(image_np):
# # #     """Convert image to circular shape"""
# # #     pil_img = Image.fromarray(cv2.cvtColor(image_np, cv2.COLOR_BGR2RGB))
    
# # #     mask = Image.new('L', pil_img.size, 0)
# # #     draw = ImageDraw.Draw(mask)
# # #     draw.ellipse((0, 0, pil_img.size[0], pil_img.size[1]), fill=255)
    
# # #     result = Image.new('RGBA', pil_img.size)
# # #     result.paste(pil_img, (0, 0), mask)
    
# # #     return cv2.cvtColor(np.array(result), cv2.COLOR_RGBA2BGR)

# # # @app.route("/detect_face", methods=["POST"])
# # # def detect_face():
# # #     if "image" not in request.files:
# # #         return "No image uploaded", 400

# # #     file = request.files["image"]
    
# # #     print(f"✅ Received image: {file.filename}, size: {len(file.read())} bytes")
# # #     file.seek(0)  # Reset file pointer

# # #     try:
# # #         image_bytes = file.read()
# # #         nparr = np.frombuffer(image_bytes, np.uint8)
# # #         img_np = cv2.imdecode(nparr, cv2.IMREAD_COLOR)
        
# # #         # Correct orientation before processing
# # #         img_np = correct_orientation(img_np, image_bytes)

# # #         gray = cv2.cvtColor(img_np, cv2.COLOR_BGR2GRAY)
# # #         faces = face_cascade.detectMultiScale(gray, 1.3, 5)

# # #         if len(faces) == 0:
# # #             return "No face detected", 404

# # #         (x, y, w, h) = faces[0]
# # #         cropped = img_np[y:y+h, x:x+w]
        
# # #         # Make circular
# # #         circular_face = make_circular(cropped)

# # #         _, jpeg = cv2.imencode('.jpg', circular_face)
# # #         return send_file(
# # #             io.BytesIO(jpeg.tobytes()),
# # #             mimetype='image/jpeg',
# # #             as_attachment=True,
# # #             download_name='circular_face.jpg'
# # #         )

# # #     except Exception as e:
# # #         return f"Error: {str(e)}", 500

# # # if __name__ == "__main__":
# # #     app.run(host="0.0.0.0", port=5000)



# #     #
# #     #
# #     #
# #     #
# #     #
# #     #
# #     #
# #     #
# #     #
# # from flask import Flask, request, send_file
# # import cv2
# # import numpy as np
# # from PIL import Image, ImageDraw, ImageOps
# # import io
# # import math
# # import exifread  # Add this import for EXIF handling

# # app = Flask(__name__)
# # face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + "haarcascade_frontalface_default.xml")

# # def correct_orientation(image_np, image_bytes):
# #     """Correct image orientation based on EXIF data"""
# #     try:
# #         pil_img = Image.open(io.BytesIO(image_bytes))
        
# #         # Check EXIF orientation
# #         exif = pil_img._getexif()
# #         if exif:
# #             for tag, value in exif.items():
# #                 if ExifTags.TAGS.get(tag) == 'Orientation':
# #                     if value == 3:
# #                         image_np = cv2.rotate(image_np, cv2.ROTATE_180)
# #                     elif value == 6:
# #                         image_np = cv2.rotate(image_np, cv2.ROTATE_90_CLOCKWISE)
# #                     elif value == 8:
# #                         # 3 хувилбарт аль нэгийг нь ашиглана
# #                         image_np = cv2.rotate(image_np, cv2.ROTATE_90_COUNTERCLOCKWISE)  # 1
# #                         # image_np = np.rot90(image_np)  # 2
# #                         # image_np = np.array(Image.fromarray(image_np).rotate(90))  # 3
# #                     break
                    
# #     except Exception as e:
# #         print(f"Orientation correction error: {str(e)}")
    
# #     return image_np



# # def make_circular(image_np):
# #     """Convert image to circular shape"""
# #     pil_img = Image.fromarray(cv2.cvtColor(image_np, cv2.COLOR_BGR2RGB))
    
# #     mask = Image.new('L', pil_img.size, 0)
# #     draw = ImageDraw.Draw(mask)
# #     draw.ellipse((0, 0, pil_img.size[0], pil_img.size[1]), fill=255)
    
# #     result = Image.new('RGBA', pil_img.size)
# #     result.paste(pil_img, (0, 0), mask)
    
# #     return cv2.cvtColor(np.array(result), cv2.COLOR_RGBA2BGR)

# # @app.route("/detect_face", methods=["POST"])
# # def detect_face():
# #     if "image" not in request.files:
# #         return "No image uploaded", 400

# #     file = request.files["image"]
    
# #     print(f"✅ Received image: {file.filename}, size: {len(file.read())} bytes")
# #     file.seek(0)  # Reset file pointer

# #     try:
# #         image_bytes = file.read()
# #         nparr = np.frombuffer(image_bytes, np.uint8)
# #         img_np = cv2.imdecode(nparr, cv2.IMREAD_COLOR)
        
# #         # Correct orientation before processing
# #         img_np = correct_orientation(img_np, image_bytes)

# #         gray = cv2.cvtColor(img_np, cv2.COLOR_BGR2GRAY)
# #         faces = face_cascade.detectMultiScale(gray, 1.3, 5)

# #         if len(faces) == 0:
# #             return "No face detected", 404

# #         (x, y, w, h) = faces[0]
# #         cropped = img_np[y:y+h, x:x+w]
        
# #         # Make circular
# #         circular_face = make_circular(cropped)

# #         _, jpeg = cv2.imencode('.jpg', circular_face)
# #         return send_file(
# #             io.BytesIO(jpeg.tobytes()),
# #             mimetype='image/jpeg',
# #             as_attachment=True,
# #             download_name='circular_face.jpg'
# #         )

# #     except Exception as e:
# #         return f"Error: {str(e)}", 500

# # if __name__ == "__main__":
# #     app.run(host="0.0.0.0", port=5000)


# from flask import Flask, request, send_file
# import cv2
# import numpy as np
# from PIL import Image, ImageDraw, ImageOps, ExifTags
# import io

# app = Flask(__name__)
# face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + "haarcascade_frontalface_default.xml")

# def correct_orientation(image_np, image_bytes):
#     """Зургийн чиглэлийг зөв болгох"""
#     try:
#         pil_img = Image.open(io.BytesIO(image_bytes))
#         exif = pil_img._getexif()
#         if exif:
#             for tag, value in exif.items():
#                 if ExifTags.TAGS.get(tag) == 'Orientation':
#                     if value == 3:
#                         image_np = cv2.rotate(image_np, cv2.ROTATE_180)
#                     elif value == 6:
#                         image_np = cv2.rotate(image_np, cv2.ROTATE_90_CLOCKWISE)
#                     elif value == 8:
#                         image_np = cv2.rotate(image_np, cv2.ROTATE_90_COUNTERCLOCKWISE)
#                     break
#     except Exception as e:
#         print(f"Orientation correction error: {str(e)}")
#     return image_np

# def make_circular(image_np):
#     """Convert image to circular shape"""
#     pil_img = Image.fromarray(cv2.cvtColor(image_np, cv2.COLOR_BGR2RGB))
    
#     mask = Image.new('L', pil_img.size, 0)
#     draw = ImageDraw.Draw(mask)
#     draw.ellipse((0, 0, pil_img.size[0], pil_img.size[1]), fill=255)
    
#     result = Image.new('RGBA', pil_img.size)
#     result.paste(pil_img, (0, 0), mask)
    
#     return cv2.cvtColor(np.array(result), cv2.COLOR_RGBA2BGR)

# @app.route("/detect_face", methods=["POST"])
# def detect_face():
#     if "image" not in request.files:
#         return "No image uploaded", 400

#     file = request.files["image"]
#     image_bytes = file.read()
#     file.seek(0)  # Reset pointer to start
#     print(f"✅ Received image: {file.filename}, size: {len(image_bytes)} bytes")

#     try:
#         nparr = np.frombuffer(image_bytes, np.uint8)
#         img_np = cv2.imdecode(nparr, cv2.IMREAD_COLOR)
        
#         # Correct orientation
#         img_np = correct_orientation(img_np, image_bytes)

#         gray = cv2.cvtColor(img_np, cv2.COLOR_BGR2GRAY)
#         faces = face_cascade.detectMultiScale(gray, 1.3, 5)

#         if len(faces) == 0:
#             return "No face detected", 404

#         (x, y, w, h) = faces[0]
#         cropped = img_np[y:y+h, x:x+w]
        
#         circular_face = make_circular(cropped)

#         _, jpeg = cv2.imencode('.jpg', circular_face)
#         return send_file(
#             io.BytesIO(jpeg.tobytes()),
#             mimetype='image/jpeg',
#             as_attachment=True,
#             download_name='circular_face.jpg'
#         )

#     except Exception as e:
#         return f"Error: {str(e)}", 500

# if __name__ == "__main__":
#     app.run(host="0.0.0.0", port=5000)



from flask import Flask, request, send_file
import cv2
import numpy as np
from PIL import Image, ImageDraw, ImageOps, ExifTags  # Added ExifTags here
import io
import math
import exifread

app = Flask(__name__)
face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + "haarcascade_frontalface_default.xml")

def correct_orientation(image_np, image_bytes):
    """Correct image orientation based on EXIF data"""
    try:
        pil_img = Image.open(io.BytesIO(image_bytes))
        
        # Check EXIF orientation
        exif = pil_img._getexif()
        if exif:
            for tag, value in exif.items():
                if ExifTags.TAGS.get(tag) == 'Orientation':
                    if value == 3:
                        image_np = cv2.rotate(image_np, cv2.ROTATE_180)
                    elif value == 6:
                        image_np = cv2.rotate(image_np, cv2.ROTATE_90_CLOCKWISE)
                    elif value == 8:
                        image_np = cv2.rotate(image_np, cv2.ROTATE_90_COUNTERCLOCKWISE)
                    break
                    
    except Exception as e:
        print(f"Orientation correction error: {str(e)}")
    
    return image_np

def make_circular(image_np):
    """Convert image to circular shape"""
    pil_img = Image.fromarray(cv2.cvtColor(image_np, cv2.COLOR_BGR2RGB))
    
    mask = Image.new('L', pil_img.size, 0)
    draw = ImageDraw.Draw(mask)
    draw.ellipse((0, 0, pil_img.size[0], pil_img.size[1]), fill=255)
    
    result = Image.new('RGBA', pil_img.size)
    result.paste(pil_img, (0, 0), mask)
    
    return cv2.cvtColor(np.array(result), cv2.COLOR_RGBA2BGR)

@app.route("/detect_face", methods=["POST"])
def detect_face():
    if "image" not in request.files:
        return "No image uploaded", 400

    file = request.files["image"]
    
    try:
        image_bytes = file.read()
        print(f"✅ Received image: {file.filename}, size: {len(image_bytes)} bytes")
        
        nparr = np.frombuffer(image_bytes, np.uint8)
        img_np = cv2.imdecode(nparr, cv2.IMREAD_COLOR)
        
        # Correct orientation before processing
        img_np = correct_orientation(img_np, image_bytes)

        gray = cv2.cvtColor(img_np, cv2.COLOR_BGR2GRAY)
        faces = face_cascade.detectMultiScale(gray, 1.3, 5)

        if len(faces) == 0:
            return "No face detected", 404

        (x, y, w, h) = faces[0]
        cropped = img_np[y:y+h, x:x+w]
        
        # Make circular
        circular_face = make_circular(cropped)

        _, jpeg = cv2.imencode('.jpg', circular_face)
        return send_file(
            io.BytesIO(jpeg.tobytes()),
            mimetype='image/jpeg',
            as_attachment=True,
            download_name='circular_face.jpg'
        )

    except Exception as e:
        print(f"Error processing image: {str(e)}")
        return f"Error: {str(e)}", 500

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)