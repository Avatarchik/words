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
	name1 = self.request.get('thing')
	#print(name1)

	"""Return a friendly HTTP greeting."""
	return 'Hello World! It Works! test 456'


@app.errorhandler(404)
def page_not_found(e):
	"""Return a custom 404 error."""
	return 'Sorry, nothing at this URL.', 404