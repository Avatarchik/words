import webapp2
from google.appengine.ext import ndb

class WOTD(ndb.Model):
	daystamp = ndb.IntegerProperty()
	word = ndb.StringProperty()
	definition = ndb.StringProperty()

wotds = [
	{ 0, "OUTVOTES", "To defeat someone or something by having a larger number of votes." },
	{ 1, "REGISTRAR", "Someone who is in charge of official records, for example in a city or a college." },
	{ 2, "WRESTLINGS", "To fight someone by holding onto and pulling or pushing someone." },
	{ 3, "MEANED", "To have a particular meaning or be used as a symbol or sign for something." },
	{ 4, "CYANIDINGS", "A very strong poison." },
	{ 5, "INSURANCES", "Protected by insurance." },
	{ 6, "ELBOW", "The joint where your arm bends." },
	{ 7, "BARBES", "The sharp curved point of a hook,arrow, etc. that prevents it from being easily pulled out." },
	{ 8, "APPAREL", "Clothing – used especially by stores or the clothing industry." },
	{ 9, "REPRESENTS", "A member of the House of Representatives, the lower House of Congress in the United States." },
]

class UploadWOTD(webapp2.RequestHandler):
	def get(self):
		self.response.write('UNKNOWN')

app = webapp2.WSGIApplication([
	('/wotd_uploader/?', UploadWOTD),
], debug=True)