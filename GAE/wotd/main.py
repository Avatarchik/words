import os
import urllib
import webapp2
from google.appengine.ext import ndb

class WOTD(ndb.Model):
	day = ndb.IntegerProperty()
	word = ndb.StringProperty()
	definition = ndb.StringProperty()

class PageNotFound(webapp2.RequestHandler):
	def get(self):
		self.response.write('Page not found!')

class GetWOTD(webapp2.RequestHandler):
	def get(self):
		day = int(self.request.get('day', -1))
		self.response.write(day)

		if day != -1:
			self.response.write("day is valid")
			todays_wotd = WOTD.query().filter(WOTD.day == day).get()
			if todays_wotd:
				self.response.write("today's wotd is valid")
				self.response.write(todays_wotd.day)
				self.response.write(todays_wotd.word)
				self.response.write(todays_wotd.definition)
				return
			else:
				self.response.write("today's wotd is invalid")
		
		self.response.write('UNKNOWN')

app = webapp2.WSGIApplication([
	('/', PageNotFound),
	('/wotd', GetWOTD),
], debug=True)