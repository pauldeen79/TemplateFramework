﻿namespace TemplateFramework.Core.StringBuilderTemplateRenderers;

public class TypedTextTransformTemplateRenderer : IBuilderTemplateRenderer<StringBuilder>
{
    public async Task<Result> TryRenderAsync(object instance, StringBuilder builder, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(builder);

        if (instance is ITextTransformTemplate textTransformTemplate)
        {
            Guard.IsNotNull(builder);

            var output = await textTransformTemplate.TransformTextAsync(cancellationToken).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(output))
            {
                builder.Append(output);
            }

            return Result.Success();
        }

        return Result.Continue();
    }
}
