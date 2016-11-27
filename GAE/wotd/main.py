import os
import urllib
import webapp2
from google.appengine.ext import ndb

class WOTD(ndb.Model):
	daystamp = ndb.IntegerProperty()
	word = ndb.StringProperty()
	definition = ndb.StringProperty()

class PageNotFound(webapp2.RequestHandler):
	def get(self):
		self.response.write('Page not found!')

class GetWOTD(webapp2.RequestHandler):
	def get(self):
		daystamp = int(self.request.get('daystamp', -1))

		if daystamp != -1:
			todays_wotd = WOTD.query(WOTD.daystamp == daystamp).get()
			if todays_wotd:
				self.response.write(todays_wotd)
				return
		
		self.response.write('UNKNOWN')

app = webapp2.WSGIApplication([
	('/', PageNotFound),
	('/wotd', GetWOTD),
], debug=True)