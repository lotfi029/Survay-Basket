﻿namespace Survay_Basket.API.Contracts.Results;

public record QuestionAnswersResponse(
    string Question,
    string Answers
);