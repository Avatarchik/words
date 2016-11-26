from flask import Flask
import datetime
from google.appengine.ext import webapp

app = Flask(__name__)
app.config['DEBUG'] = True

# Note: We don't need to call run() since our application is embedded within
# the App Engine WSGI application server.

@app.route('/')
def hello():
	# GET
	x = MyHandler()
	print x.get()

    """Return a friendly HTTP greeting."""
    return 'Hello World! It Works! test 456'

class MyHandler(webapp.RequestHandler):
    def get(self):
        name1 = self.request.get_all("thing")
        return name1


@app.errorhandler(404)
def page_not_found(e):
    """Return a custom 404 error."""
    return 'Sorry, nothing at this URL.', 404
