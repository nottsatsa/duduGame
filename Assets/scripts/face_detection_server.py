# from flask import Flask, request, send_file
# import cv2
# import numpy as np
# from PIL import Image
# import io

# app = Flask(__name__)
# face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + "haarcascade_frontalface_default.xml")

# @app.route("/detect_face", methods=["POST"])
# def detect_face():
#     if "image" not in request.files:
#         return "No image uploaded", 400

#     file = request.files["image"]
#     image_bytes = file.read()
#     nparr = np.frombuffer(image_bytes, np.uint8)
#     img_np = cv2.imdecode(nparr, cv2.IMREAD_COLOR)

#     gray = cv2.cvtColor(img_np, cv2.COLOR_BGR2GRAY)
#     faces = face_cascade.detectMultiScale(gray, 1.3, 5)

#     if len(faces) == 0:
#         return "No face detected", 404

#     (x, y, w, h) = faces[0]
#     cropped = img_np[y:y+h, x:x+w]

#     _, jpeg = cv2.imencode('.jpg', cropped)
#     return send_file(io.BytesIO(jpeg.tobytes()), mimetype='image/jpeg')

# if __name__ == "__main__":
#     app.run(host="0.0.0.0", port=5000)


#     print(f"✅ Хүлээн авсан зураг: {file.filename}, хэмжээ: {len(file.read())} bytes")
#     file.seek(0)  # read хийсний дараа reset хийх


# ##############################################################################
# # from flask import Flask, request, send_file
# # import cv2
# # import numpy as np
# # from PIL import Image, ImageDraw
# # import io
# # import math

# # app = Flask(__name__)
# # face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + "haarcascade_frontalface_default.xml")

# # def make_circular(image_np):
# #     """Зургийг дугуй хэлбэртэй болгох функц"""
# #     pil_img = Image.fromarray(cv2.cvtColor(image_np, cv2.COLOR_BGR2RGB))
    
# #     # Дугуй маск үүсгэх
# #     mask = Image.new('L', pil_img.size, 0)
# #     draw = ImageDraw.Draw(mask)
# #     draw.ellipse((0, 0, pil_img.size[0], pil_img.size[1]), fill=255)
    
# #     # Зургийг дугуй хэлбэртэй болгох
# #     result = Image.new('RGBA', pil_img.size)
# #     result.paste(pil_img, (0, 0), mask)
    
# #     return cv2.cvtColor(np.array(result), cv2.COLOR_RGBA2BGR)

# # @app.route("/detect_face", methods=["POST"])
# # def detect_face():
# #     if "image" not in request.files:
# #         return "Зураг оруулаагүй байна", 400

# #     file = request.files["image"]
# #     print(f"✅ Хүлээн авсан зураг: {file.filename}, хэмжээ: {len(file.read())} bytes")
# #     file.seek(0)  # Файлыг дахин уншихад бэлтгэх
    
# #     try:
# #         # Зургийг унших
# #         image_bytes = file.read()
# #         nparr = np.frombuffer(image_bytes, np.uint8)
# #         img_np = cv2.imdecode(nparr, cv2.IMREAD_COLOR)

# #         # Нүүр илрүүлэх
# #         gray = cv2.cvtColor(img_np, cv2.COLOR_BGR2GRAY)
# #         faces = face_cascade.detectMultiScale(gray, 1.3, 5)

# #         if len(faces) == 0:
# #             return "Нүүр илрүүлээгүй", 404

# #         # Эхний нүүрний хэсгийг таслах
# #         (x, y, w, h) = faces[0]
# #         cropped = img_np[y:y+h, x:x+w]
        
# #         # Дугуй хэлбэртэй болгох
# #         circular_face = make_circular(cropped)

# #         # JPEG форматаар буцаах
# #         _, jpeg = cv2.imencode('.jpg', circular_face)
# #         return send_file(io.BytesIO(jpeg.tobytes()), mimetype='image/jpeg')
    
# #     except Exception as e:
# #         return f"Алдаа гарлаа: {str(e)}", 500

# # if __name__ == "__main__":
# #     app.run(host="0.0.0.0", port=5000)

#######################################################################
#yg goy tsaraigaa toirogoor haruulna
from flask import Flask, request, send_file
import cv2
import numpy as np
from PIL import Image, ImageDraw
import io
import math

app = Flask(__name__)
face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + "haarcascade_frontalface_default.xml")

def make_circular(image_np):
    """Зургийг дугуй хэлбэртэй болгох"""
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
    
    # Файлын мэдээллийг хэвлэх
    print(f"✅ Received image: {file.filename}, size: {len(file.read())} bytes")
    file.seek(0)  # Файлыг дахин уншихад бэлтгэх

    try:
        image_bytes = file.read()
        nparr = np.frombuffer(image_bytes, np.uint8)
        img_np = cv2.imdecode(nparr, cv2.IMREAD_COLOR)

        gray = cv2.cvtColor(img_np, cv2.COLOR_BGR2GRAY)
        faces = face_cascade.detectMultiScale(gray, 1.3, 5)

        if len(faces) == 0:
            return "No face detected", 404

        (x, y, w, h) = faces[0]
        cropped = img_np[y:y+h, x:x+w]
        
        # Дугуй хэлбэртэй болгох
        circular_face = make_circular(cropped)

        _, jpeg = cv2.imencode('.jpg', circular_face)
        return send_file(
            io.BytesIO(jpeg.tobytes()),
            mimetype='image/jpeg',
            as_attachment=True,
            download_name='circular_face.jpg'
        )

    except Exception as e:
        return f"Error: {str(e)}", 500

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)