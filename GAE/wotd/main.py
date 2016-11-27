import os
import urllib
import webapp2
from google.appengine.ext import ndb

class WordDefinition(ndb.Model):

	day = ndb.IntegerProperty(indexed=True)
	word = ndb.StringProperty(indexed=False)
	definition = ndb.StringProperty(indexed=False)

class PageNotFound(webapp2.RequestHandler):

	def get(self):
		self.response.write('Page not found. 404')

class WOTD(webapp2.RequestHandler):

	def get(self):
		day = int(self.request.get('day', -1))
		self.response.write(day)

		if day != -1:
			self.response.write("day is valid")
			todays_wotd_query = WordDefinition.query(WordDefinition.day == day)
			todays_wotd = todays_wotd_query.fetch(1)
			self.response.write(todays_wotd[0].day)
			self.response.write(todays_wotd[0].word)
			self.response.write(todays_wotd[0].definition)
			return
		
		self.response.write('UNKNOWN')

app = webapp2.WSGIApplication([
	('/', PageNotFound),
	('/wotd', WOTD),
], debug=True)