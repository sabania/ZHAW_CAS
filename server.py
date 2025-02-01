from http.server import SimpleHTTPRequestHandler, HTTPServer

class CustomHandler(SimpleHTTPRequestHandler):
    def end_headers(self):
        # Allow embedding in iframes
        self.send_header("X-Frame-Options", "ALLOWALL")  # Allow all domains (not secure for production)
        self.send_header("Content-Security-Policy", "frame-ancestors *")  # Allow embedding
        self.send_header("Access-Control-Allow-Origin", "*")  # Allow cross-origin access
        super().end_headers()

if __name__ == "__main__":
    PORT = 8000  # Change if needed
    server_address = ("", PORT)
    httpd = HTTPServer(server_address, CustomHandler)
    print(f"Serving on http://localhost:{PORT}")
    httpd.serve_forever()
