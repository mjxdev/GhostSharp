using GhostSharp.Entities;
using GhostSharp.Enums;
using GhostSharp.QueryParams;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GhostSharp.Tests.IntegrationTests
{
    [TestFixture]
    public class GetAuthorIntgTests : TestBase
    {
        private const int MINIMUM_POST_COUNT_THRESHHOLD = 200;

        private GhostAPI auth;

        [SetUp]
        public void SetUp()
        {
            auth = new GhostAPI(Host, ValidApiKey);
        }

        [Test]
        public void GetAuthorById_ReturnsMatchingAuthor()
        {
            var author = auth.GetAuthorById(ValidAuthor1Id);

            Assert.AreEqual(ValidAuthor1Id, author.Id);
            Assert.AreEqual(ValidAuthor1Slug, author.Slug);
            Assert.AreEqual(ValidAuthor1Name, author.Name);
            Assert.AreEqual(ValidAuthor1Url, author.Url);
            Assert.IsNotNull(author.Bio);
            Assert.IsNull(author.Count);
        }

        [Test]
        public void GetAuthorById_ReturnsPostCount_WhenIncludingCountPosts()
        {
            var author = auth.GetAuthorById(ValidAuthor1Id, new AuthorQueryParams { IncludePostCount = true });

            Assert.AreEqual(ValidAuthor1Id, author.Id);
            Assert.AreEqual(ValidAuthor1Slug, author.Slug);
            Assert.AreEqual(ValidAuthor1Name, author.Name);
            Assert.AreEqual(ValidAuthor1Url, author.Url);
            Assert.IsNotNull(author.Bio);
            Assert.Greater(author.Count.Posts, MINIMUM_POST_COUNT_THRESHHOLD);
        }

        [Test]
        public void GetAuthorById_IgnoresFieldsParameter_WhenFieldsSpecified_ForIndividualRequest()
        {
            var author = auth.GetAuthorById(ValidAuthor1Id, new AuthorQueryParams { Fields = AuthorFields.Id | AuthorFields.Slug });

            Assert.AreEqual(ValidAuthor1Id, author.Id);
            Assert.AreEqual(ValidAuthor1Slug, author.Slug);
            Assert.AreEqual(ValidAuthor1Name, author.Name);
            Assert.AreEqual(ValidAuthor1Url, author.Url);
            Assert.IsNotNull(author.Bio);
            Assert.IsNull(author.Count);
        }

        [Test]
        public void GetAuthorBySlug_ReturnsMatchingAuthor()
        {
            var author = auth.GetAuthorBySlug(ValidAuthor1Slug);

            Assert.AreEqual(ValidAuthor1Id, author.Id);
            Assert.AreEqual(ValidAuthor1Slug, author.Slug);
            Assert.AreEqual(ValidAuthor1Name, author.Name);
            Assert.AreEqual(ValidAuthor1Url, author.Url);
            Assert.IsNotNull(author.Bio);
            Assert.IsNull(author.Count);
        }

        [Test]
        public void GetAuthorBySlug_ReturnsPostCount_WhenIncludingCountPosts()
        {
            var author = auth.GetAuthorBySlug(ValidAuthor1Slug, new AuthorQueryParams { IncludePostCount = true });

            Assert.AreEqual(ValidAuthor1Id, author.Id);
            Assert.AreEqual(ValidAuthor1Slug, author.Slug);
            Assert.AreEqual(ValidAuthor1Name, author.Name);
            Assert.AreEqual(ValidAuthor1Url, author.Url);
            Assert.IsNotNull(author.Bio);
            Assert.Greater(author.Count.Posts, MINIMUM_POST_COUNT_THRESHHOLD);
        }

        [Test]
        public void GetAuthorBySlug_IgnoresFieldsParameter_WhenFieldsSpecified_ForIndividualRequest()
        {
            var author = auth.GetAuthorBySlug(ValidAuthor1Slug, new AuthorQueryParams { Fields = AuthorFields.Id | AuthorFields.Slug });

            Assert.AreEqual(ValidAuthor1Id, author.Id);
            Assert.AreEqual(ValidAuthor1Slug, author.Slug);
            Assert.AreEqual(ValidAuthor1Name, author.Name);
            Assert.AreEqual(ValidAuthor1Url, author.Url);
            Assert.IsNotNull(author.Bio);
            Assert.IsNull(author.Count);
        }

        [Test]
        public void GetAuthorBySlug_Throws_WhenAuthorHasNoPublishedPosts()
        {
            var ex = Assert.Throws<GhostSharpException>(() => auth.GetAuthorBySlug(ValidAuthorWithNoPublishedPostsSlug));

            Assert.AreEqual("Author not found.", ex.Message);
        }

        [Test]
        public void GetAuthors_ReturnsLimitedAuthors_WhenLimitSpecified()
        {
            var auth = new GhostAPI(Host, ValidApiKey);

            var authorResponse = auth.GetAuthors(new AuthorQueryParams { Limit = 1, Fields = AuthorFields.Id });

            Assert.AreEqual(1, authorResponse.Authors.Count);
        }

        [Test]
        public void GetAuthors_ReturnsLimitedFields_WhenFieldsSpecified()
        {
            var auth = new GhostAPI(Host, ValidApiKey);

            var author = auth.GetAuthors(new AuthorQueryParams { Limit = 1, Fields = AuthorFields.Id }).Authors[0];

            Assert.IsNull(author.Bio);
            Assert.IsNull(author.Count);
            Assert.IsNull(author.CoverImage);
            Assert.IsNull(author.Facebook);
            Assert.IsNull(author.Location);
            Assert.IsNull(author.MetaDescription);
            Assert.IsNull(author.MetaTitle);
            Assert.IsNull(author.Name);
            Assert.IsNull(author.ProfileImage);
            Assert.IsNull(author.Slug);
            Assert.IsNull(author.Twitter);
            Assert.IsNull(author.Website);

            Assert.IsNotNull(author.Id);
            Assert.IsNotNull(author.Url);  // for some reason, url is always included in the response
        }

        [Test]
        public void GetAuthors_ReturnsPostCount_WhenIncludingCountPosts()
        {
            var auth = new GhostAPI(Host, ValidApiKey);

            var author = auth.GetAuthors(new AuthorQueryParams { Limit = 1, IncludePostCount = true }).Authors[0];

            Assert.IsNotNull(author.Count.Posts);
        }

        [Test]
        public void GetAuthors_ReturnsAllAuthors_WhenNoLimitIsTrue()
        {
            var auth = new GhostAPI(Host, ValidApiKey);

            var authorResponse = auth.GetAuthors(new AuthorQueryParams { Limit = 1, NoLimit = true, Fields = AuthorFields.Id });

            Assert.GreaterOrEqual(authorResponse.Authors.Count, 2);
        }

        [Test]
        public void GetAuthors_ReturnsExpectedAuthor_WhenOrderingByField()
        {
            var auth = new GhostAPI(Host, ValidApiKey);

            var author = auth.GetAuthors(new AuthorQueryParams { Limit = 1, Order = new List<Tuple<AuthorFields, OrderDirection>> { Tuple.Create(AuthorFields.Slug, OrderDirection.desc) } }).Authors[0];

            Assert.AreEqual(ValidAuthor2Id, author.Id);
            Assert.AreEqual(ValidAuthor2Slug, author.Slug);
            Assert.AreEqual(ValidAuthor2Name, author.Name);
            Assert.AreEqual(ValidAuthor2Url, author.Url);
        }

        [Test]
        public void GetAuthors_ReturnsExpectedAuthor_WhenGettingSecondPage()
        {
            var auth = new GhostAPI(Host, ValidApiKey);

            var author = auth.GetAuthors(new AuthorQueryParams { Limit = 1, Page = 2, Order = new List<Tuple<AuthorFields, OrderDirection>> { Tuple.Create(AuthorFields.Slug, OrderDirection.asc) } }).Authors[0];

            Assert.AreEqual(ValidAuthor2Id, author.Id);
            Assert.AreEqual(ValidAuthor2Slug, author.Slug);
            Assert.AreEqual(ValidAuthor2Name, author.Name);
            Assert.AreEqual(ValidAuthor2Url, author.Url);
        }

        [Test]
        public void GetAuthors_ReturnsExpectedAuthors_WhenApplyingFilter()
        {
            var auth = new GhostAPI(Host, ValidApiKey);

            var authorResponse = auth.GetAuthors(new AuthorQueryParams { Filter = "slug:[grant2]" });
            Assert.AreEqual(1, authorResponse.Authors.Count);

            var author = authorResponse.Authors[0];

            Assert.AreEqual(ValidAuthor2Id, author.Id);
            Assert.AreEqual(ValidAuthor2Slug, author.Slug);
            Assert.AreEqual(ValidAuthor2Name, author.Name);
            Assert.AreEqual(ValidAuthor2Url, author.Url);
        }

        [TestCase(ExceptionLevel.Ghost)]
        [TestCase(ExceptionLevel.All)]
        public void GetAuthors_ThrowsException_WhenKeyIsInvalid(ExceptionLevel exceptionLevel)
        {
            var auth = new GhostAPI(Host, InvalidApiKey) { ExceptionLevel = exceptionLevel };

            var ex = Assert.Throws<GhostSharpException>(() => auth.GetAuthors());
            Assert.IsNotEmpty(ex.Errors);
            Assert.AreEqual("Unknown Content API Key", ex.Errors[0].Message);
        }

        [TestCase(ExceptionLevel.None)]
        [TestCase(ExceptionLevel.NonGhost)]
        public void GetAuthors_ReturnsNull_WhenKeyIsInvalid_AndGhostExceptionsSuppressed(ExceptionLevel exceptionLevel)
        {
            var auth = new GhostAPI(Host, InvalidApiKey) { ExceptionLevel = exceptionLevel };

            Assert.IsNull(auth.GetAuthors());
            Assert.IsNotNull(auth.LastException);
        }

        [TestCase(ExceptionLevel.Ghost)]
        [TestCase(ExceptionLevel.All)]
        public void GetAuthorById_ThrowsGhostSharpException_WhenIdIsInvalid(ExceptionLevel exceptionLevel)
        {
            auth.ExceptionLevel = exceptionLevel;

            var ex = Assert.Throws<GhostSharpException>(() => auth.GetAuthorById(InvalidAuthorId));

            Assert.IsNotEmpty(ex.Errors);
            Assert.AreEqual("Author not found.", ex.Errors[0].Message);
        }

        [TestCase(ExceptionLevel.None)]
        [TestCase(ExceptionLevel.NonGhost)]
        public void GetAuthorById_ReturnsNull_WhenKeyIsInvalid_AndGhostExceptionsSuppressed(ExceptionLevel exceptionLevel)
        {
            auth.ExceptionLevel = exceptionLevel;

            Assert.IsNull(auth.GetAuthorById(InvalidAuthorId));
        }

        [TestCase(ExceptionLevel.Ghost)]
        [TestCase(ExceptionLevel.All)]
        public void GetAuthorBySlug_ThrowsGhostSharpException_WhenSlugIsInvalid(ExceptionLevel exceptionLevel)
        {
            auth.ExceptionLevel = exceptionLevel;

            var ex = Assert.Throws<GhostSharpException>(() => auth.GetAuthorBySlug(InvalidAuthorSlug));

            Assert.IsNotEmpty(ex.Errors);
            Assert.AreEqual("Author not found.", ex.Errors[0].Message);
        }

        [TestCase(ExceptionLevel.None)]
        [TestCase(ExceptionLevel.NonGhost)]
        public void GetAuthorBySlug_ReturnsNull_WhenKeyIsInvalid_AndGhostExceptionsSuppressed(ExceptionLevel exceptionLevel)
        {
            auth.ExceptionLevel = exceptionLevel;

            Assert.IsNull(auth.GetAuthorBySlug(InvalidAuthorSlug));
        }
    }
}
